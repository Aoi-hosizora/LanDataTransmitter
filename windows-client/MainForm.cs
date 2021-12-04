using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using Channels = System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanDataTransmitter {

    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private static MainForm _instance;

        public static MainForm Instance {
            get {
                if (_instance == null) {
                    _instance = new MainForm();
                }
                return _instance;
            }
        }

        private Channels.Channel<PullTextReply> pullTextChannel;

        private void MainForm_Load(object sender, EventArgs e) {
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                var channel = new Channel(Global.grpcService.Address, Global.grpcService.Port, ChannelCredentials.Insecure);
                var client = new Transmitter.TransmitterClient(channel);
                var pullReply = client.PullText(new PullTextRequest());
                Global.pullTextReader = pullReply.ResponseStream;
                var th = new Thread(async () => {
                    while (await Global.pullTextReader.MoveNext()) {
                        var reply = Global.pullTextReader.Current;
                        Invoke(new Action(() => {
                            lbRecord.Items.Add("From server: " + reply.Message);
                        }));
                    }
                });
                th.Start();
            } else {
                pullTextChannel = Channels.Channel.CreateBounded<PullTextReply>(1);
                Global.grpcService.SetupTransmitter(onDisplay: (req) => {
                    Invoke(new Action(() => {
                        lbRecord.Items.Add("From client: " + req.Message);
                    }));
                }, pullTextChannel: pullTextChannel);
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                var th = new Thread(() => {
                    Global.grpcService.Disconnect();
                });
                th.Start();
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            if (Global.behavior == ApplicationBehavior.AS_CLIENT) {
                var th = new Thread(() => {
                    var channel = new Channel(Global.grpcService.Address, Global.grpcService.Port, ChannelCredentials.Insecure);
                    var client = new Transmitter.TransmitterClient(channel);
                    client.PushText(new PushTextRequest { Message = edtText.Text });
                    edtText.Text = "";
                });
                th.Start();
            } else {
                var th = new Thread(async () => {
                    var reply = new PullTextReply { Message = edtText.Text };
                    await pullTextChannel.Writer.WriteAsync(reply);
                });
                th.Start();
            }
        }
    }
}

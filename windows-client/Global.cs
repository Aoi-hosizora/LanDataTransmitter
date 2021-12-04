using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanDataTransmitter {

    enum ApplicationBehavior {
        AS_SERVER,
        AS_CLIENT,
    }

    static class Global {

        public static ApplicationBehavior behavior;
        public static GrpcService grpcService;

        // server only
        public static bool clientBind;

        // client only
        public static IAsyncStreamReader<PullTextReply> pullTextReader;

    }
}

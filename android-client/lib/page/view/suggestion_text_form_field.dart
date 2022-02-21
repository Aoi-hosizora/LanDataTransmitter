import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_typeahead/flutter_typeahead.dart';
import 'package:lan_data_transmitter/util/extensions.dart';

class SuggestionTextFormField extends StatefulWidget {
  final TextFieldConfiguration textFieldConfiguration;
  final FormFieldValidator<String>? validator;
  final double suggestionsBoxVerticalOffset;
  final SuggestionsBoxDecoration suggestionsBoxDecoration;
  final Function(String suggestion) onSuggestionSelected;
  final Function(String suggestion) onSuggestionDeleted;
  final FutureOr<List<String>> Function(String pattern) suggestionsCallback;

  const SuggestionTextFormField({
    Key? key,
    required this.textFieldConfiguration,
    this.validator,
    this.suggestionsBoxVerticalOffset = 5.0,
    this.suggestionsBoxDecoration = const SuggestionsBoxDecoration(),
    required this.onSuggestionSelected,
    required this.onSuggestionDeleted,
    required this.suggestionsCallback,
  }) : super(key: key);

  @override
  _SuggestionTextFormFieldState createState() => _SuggestionTextFormFieldState();
}

class _SuggestionTextFormFieldState extends State<SuggestionTextFormField> {
  final _suggestionController = SuggestionsBoxController();

  @override
  Widget build(BuildContext context) {
    return TypeAheadFormField<String>(
      textFieldConfiguration: widget.textFieldConfiguration,
      enabled: widget.textFieldConfiguration.enabled,
      validator: widget.validator,
      // <<<<<<
      getImmediateSuggestions: false,
      hideOnLoading: true,
      hideOnEmpty: true,
      hideOnError: true,
      hideSuggestionsOnKeyboardHide: true,
      suggestionsBoxVerticalOffset: widget.suggestionsBoxVerticalOffset,
      suggestionsBoxDecoration: widget.suggestionsBoxDecoration,
      // <<<<<<
      suggestionsBoxController: _suggestionController,
      suggestionsCallback: widget.suggestionsCallback,
      onSuggestionSelected: (_) {},
      itemBuilder: (context, item) => InkWell(
        child: Container(
          padding: EdgeInsets.symmetric(horizontal: 12, vertical: 12),
          child: Text(
            item,
            style: Theme.of(context).textTheme.subtitle1,
          ),
        ),
        onTap: () {
          _suggestionController.close();
          widget.onSuggestionSelected(item);
        },
        onLongPress: () async {
          var ok = await showQuestion(
            title: '删除确认',
            message: '是否删除历史记录 "$item"',
            trueText: '删除',
            falseText: '取消',
          );
          if (ok) {
            widget.onSuggestionDeleted(item);
          }
        },
      ),
    );
  }
}

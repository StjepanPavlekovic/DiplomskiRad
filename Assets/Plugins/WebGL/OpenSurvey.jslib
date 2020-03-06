mergeInto(LibraryManager.library, {

  ShowSurveyLink: function (str) {
    window.alert(Pointer_stringify(str));
  },

  OpenSurvey: function (str) {
    window.open(Pointer_stringify(str));
  },

});
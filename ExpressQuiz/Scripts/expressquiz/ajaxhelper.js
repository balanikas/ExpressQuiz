var ExpressQuiz;
(function (ExpressQuiz) {
    var AjaxHelper = (function () {
        function AjaxHelper() {
        }
        AjaxHelper.createRequestionVerificationTokenHeader = function () {
            var headers = {};
            var token = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

            if (token === undefined) {
                token = $('input[name=__RequestVerificationToken]').val();
            }
            headers["__RequestVerificationToken"] = token;
            return headers;
        };
        return AjaxHelper;
    })();
    ExpressQuiz.AjaxHelper = AjaxHelper;
})(ExpressQuiz || (ExpressQuiz = {}));

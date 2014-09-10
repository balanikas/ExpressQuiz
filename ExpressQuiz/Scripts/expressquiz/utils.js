var ExpressQuiz;
(function (ExpressQuiz) {
    var Utils = (function () {
        function Utils() {
        }
        Utils.togglePreventLeavingPage = function (enable) {
            if (enable) {
                $(window).on('beforeunload', function () {
                    return 'All unsaved changes will be lost if you leave or refresh the page.';
                });
            } else {
                $(window).off('beforeunload');
            }
        };

        Utils.voteQuestion = function (id, vote, headers) {
            $.ajax({
                url: '/Rating/RateQuestion/' + id + "?vote=" + vote,
                type: "POST",
                cache: false,
                headers: headers,
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        };
        return Utils;
    })();
    ExpressQuiz.Utils = Utils;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=utils.js.map

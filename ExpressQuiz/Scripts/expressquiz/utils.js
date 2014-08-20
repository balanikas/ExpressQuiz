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
        return Utils;
    })();
    ExpressQuiz.Utils = Utils;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=utils.js.map

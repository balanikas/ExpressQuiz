var ExpressQuiz;
(function (ExpressQuiz) {
    var Quiz = (function () {
        function Quiz(id, desc, questions) {
            this.id = id;
            this.desc = desc;
            this.questions = questions;
        }
        return Quiz;
    })();
    ExpressQuiz.Quiz = Quiz;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=quiz.js.map

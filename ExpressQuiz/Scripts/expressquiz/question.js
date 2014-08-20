var ExpressQuiz;
(function (ExpressQuiz) {
    var Question = (function () {
        function Question(id, text, answers) {
            this.id = id;
            this.text = text;
            this.answers = answers;
        }
        return Question;
    })();
    ExpressQuiz.Question = Question;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=question.js.map

var ExpressQuiz;
(function (ExpressQuiz) {
    var Answer = (function () {
        function Answer(id, text, isCorrect) {
            this.id = id;
            this.text = text;
            this.isCorrect = isCorrect;
        }
        return Answer;
    })();
    ExpressQuiz.Answer = Answer;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=answer.js.map

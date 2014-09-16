var ExpressQuiz;
(function (ExpressQuiz) {
    "use strict";

    var UserAnswer = (function () {
        function UserAnswer(questionId, answerId) {
            this.questionId = questionId;
            this.answerId = answerId;
        }
        return UserAnswer;
    })();
    ExpressQuiz.UserAnswer = UserAnswer;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=useranswer.js.map

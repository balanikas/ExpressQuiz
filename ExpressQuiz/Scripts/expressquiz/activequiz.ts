
module ExpressQuiz {
    "use strict";

    export class ActiveQuiz {

        private counter: ExpressQuiz.CountDown;
        private $counter: JQuery;
        private $answers: JQuery;
        private $questions: JQuery;
        private $progressBar: JQuery;
        private $voting: JQuery;
        private $done: JQuery;
        private $pager: JQuery;
        private quizId: number;
        private totalTime: number;
        private runtime: ExpressQuiz.Runtime;

        constructor(public options: any) {

            this.$counter = options.$counter;
            this.$answers = options.$answers;
            this.$questions = options.$questions;
            this.$progressBar = options.$progressBar;
            this.$voting = options.$voting;
            this.$done = options.$done;
            this.$pager = options.$pager;
            this.quizId = options.quizId;
            this.totalTime = options.totalTime;


            this.init();

            ExpressQuiz.Utils.togglePreventLeavingPage(true);

        }

        private onCounterUpdate = (remainingSeconds: number) => {
            this.$counter.text(remainingSeconds + " seconds left");

            var timeLeftPercent = (remainingSeconds / this.totalTime) * 100;
            this.$counter.width(timeLeftPercent + "%");
        };

        private onCounterEnd = (): void => {
            this.sendResults();
        };

        private onQuizFinished = (): void => {
            this.counter.stop();
            this.sendResults();
        };

        private onAnswerSelected = (): void => {
            var a = $("input[name=optionsRadios]:checked", this.$answers).val();
            this.runtime.setAnswer(a);
            var value = this.runtime.getProgress();
            this.$progressBar.css("width", value + "%").attr("aria-valuenow", value);
        };

        private onPageChanged = (event, num: number): void => {

            var a = $("input[name=optionsRadios]:checked", this.$answers).val();
            this.runtime.setAnswer(a);
            this.runtime.setActiveQuestion(num - 1);
            var question = this.runtime.getQuestion(num - 1);
            this.loadQuestion(question, this.runtime.getAnswer());
        };

        private onVoteCast = (data: any) => {

            var vote = 0;
            if (data.upvoted) {
                vote = 1;
            } else if (data.downvoted) {
                vote = -1;
            }

            this.runtime.getActiveQuestion().vote = vote;

            this.$voting.css("pointer-events", "none");
            $.ajax({
                url: "/Rating/RateQuestion/",
                type: "POST",
                cache: false,
                data: JSON.stringify({ "id": data.id, "vote": vote }),
                headers: ExpressQuiz.AjaxHelper.createRequestionVerificationTokenHeader(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                complete: (): void => {
                    setTimeout((): void => {
                        this.$voting.css("pointer-events", "");
                    }, 1000);
                }


            });
        };

        private initPaging(): void {

            ExpressQuiz.Ui.toPager(this.$pager, this.runtime.quiz.Questions.length, this.onPageChanged);

            this.loadQuestion(this.runtime.quiz.Questions[0], this.runtime.getAnswer());
        }

        private initVoting(question: any): void {

            var upvote = $("<a/>").addClass("upvote");
            var downvote = $("<a/>").addClass("downvote");
            var container = $("<div />")
                .append(upvote)
                .append(downvote)
                .addClass("upvote-superuser")
                .addClass("upvote");
            this.$voting.empty().append(container);

            var upVoted = question.vote === 1;
            var downVoted = question.vote === -1;

            container.upvote({ id: question.QuestionId, callback: this.onVoteCast, upvoted: upVoted, downvoted: downVoted });
        }


        private loadQuestion(question, answerId): void {

            this.$answers.empty();

            for (var i = 0; i < question.Answers.length; i++) {
                var checked = answerId == question.Answers[i].AnswerId ? "checked" : "";

                var div = $(" <div class='radio'/>");
                var label = $("<label/>");
                var input = $("<input type='radio' " + checked + " name='optionsRadios' id=" + i + " value=" + i + ">");

                label.append(input);
                label.append(question.Answers[i].Text);

                div.append(label);
                this.$answers.append(div);
            }

            $("input[type=radio][name=optionsRadios]").on("change",
                this.onAnswerSelected
            );


            this.$questions.empty().append($("<textarea class='pagedown hidden' >" +
                question.Text + "</textarea>"));

            ExpressQuiz.Ui.toTextEditor($("textarea.pagedown"), true);

            this.initVoting(question);
        }

        private initCounter = ($counter, totalTime) => {

            this.counter = new ExpressQuiz.CountDown({
                seconds: totalTime,
                onUpdateStatus: this.onCounterUpdate,
                onCounterEnd: this.onCounterEnd
            });

            this.counter.start();
        };

        private init(): void {

            $.ajax({
                url: "/ActiveQuiz/GetQuiz/" + this.quizId,
                type: "GET",
                error: (jqXHR: JQueryXHR, textStatus: string, errorThrown: string) => {
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    location.href = "/Home/Error/?message=" + errorThrown;
                },
                success: (data: any) => {
                    this.runtime = new ExpressQuiz.Runtime(data);
                    this.initPaging();
                    this.initCounter(this.$counter, this.totalTime);
                    this.$done.click(this.onQuizFinished);
                    this.counter.start();
                }
            });
        }

        sendResults(): void {

            this.$done.addClass("active");
            this.$counter.removeClass("active");

            var result: any;
            result =
            {
                QuizId: this.quizId,
                UserAnswers: this.runtime.getResult(),
                EllapsedTime: this.counter.getRemainingTime()
            };

            $.ajax({
                url: "/ActiveQuiz/PostResult/",
                type: "POST",
                cache: false,
                headers: ExpressQuiz.AjaxHelper.createRequestionVerificationTokenHeader(),
                data: JSON.stringify(result),
                dataType: "json",
                contentType: "application/json; charset=utf-8",

                error: (jqXHR: JQueryXHR, textStatus: string, errorThrown: string) => {
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    location.href = "/Home/Error/?message=" + errorThrown;
                },
                success: (data: any) => {
                    var url = "/QuizReview/Index/" + data;
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    window.location.href = url;
                },
                complete: () => { this.$done.addClass("active"); }

            });
        }
    }
}
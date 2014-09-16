module ExpressQuiz {
    "use strict";

    export class Utils {

        public static togglePreventLeavingPage(enable: boolean): void {
            if (enable) {
                $(window).on("beforeunload", () =>
                    "All unsaved changes will be lost if you leave or refresh the page.");
            } else {
                $(window).off("beforeunload");
            }
        }

        public static voteQuestion(id: number, vote: number, headers: any): void {
            $.ajax({
                url: "/Rating/RateQuestion/" + id + "?vote=" + vote,
                type: "POST",
                cache: false,
                headers: headers,
                dataType: "json",
                contentType: "application/json; charset=utf-8"


            });
        }
    }
}
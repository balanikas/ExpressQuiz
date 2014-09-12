

module ExpressQuiz {

    export class AjaxHelper {


        static createRequestionVerificationTokenHeader() {
            var headers: any;
            headers = {};
            var token = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

            if (token === undefined) {
                token = $('input[name=__RequestVerificationToken]').val();
            }
            headers["__RequestVerificationToken"] = token;
           
            return headers;
        }

    }




}


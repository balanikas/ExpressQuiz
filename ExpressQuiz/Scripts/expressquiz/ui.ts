
module ExpressQuiz {
    "use strict";

    export class Ui {


        static toTextEditor(selector: JQuery, readonly?: boolean): void {

            selector.pagedownBootstrap();

            $(".wmd-preview").addClass("well");

            if (readonly) {
                $(".wmd-button-bar").hide();
            }

        }

        static toVoter(selector: JQuery, url: string, id: number, upVoted: boolean, downVoted: boolean): void {

            var callback = data => {

                var vote = 0;
                if (data.upvoted) {
                    vote = 1;
                } else if (data.downvoted) {
                    vote = -1;
                }

                selector.css("pointer-events", "none");
                $.ajax({
                    url: url,
                    type: "POST",
                    cache: false,
                    data: JSON.stringify({ "id": data.id, "vote": vote }),
                    headers: ExpressQuiz.AjaxHelper.createRequestionVerificationTokenHeader(),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    complete: (): void=> {
                        setTimeout((): void=> {
                            selector.css("pointer-events", "");
                        }, 1000);
                    }


                });
            };

            selector.upvote({ id: id, callback: callback, upvoted : upVoted, downvoted: downVoted });

        }
     
        static toPager(selector: JQuery, pageCount: number, callback: any): void {

            if (pageCount <= 1) {
                return;
            }

            selector.bootpag({
                total: pageCount,
                page: 1,
                leaps: false,
                maxVisible: 10,
                next: "next",
                prev: "previous",
            });

            selector.on("page", callback);
        }

        static toReorderableTable(selector: JQuery, order: JQuery): void {

            selector.tableDnD({
                onDrop: (table, row) => {
                    var rows = table.tBodies[0].rows;
                    var ids = "";
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i] !== undefined) {
                            ids += rows[i].id + ",";
                        }
                    }
                    order.val(ids);
                }
            });   
            
            $("#" + selector.attr("id") + " tr") .hover(function () {
                $(this.cells[0]).addClass('showDragHandle');
            }, function () {
                $(this.cells[0]).removeClass('showDragHandle');
            });         
        }

        static toSlider(selector: JQuery, updateTarget: JQuery): void {
            
            selector.slider({
                formater: value => Math.round(value)
            });

            selector.on('slide', (slideEvt : any) => {
                
                updateTarget.attr("value", Math.round(slideEvt.value));
            });
        }


        static handleAjaxError(jqXhr?: JQueryXHR): void {
            alert(jqXhr.responseText);
            //location.href = "/Home/Error/?message=" + errorThrown;
        }

    }


}


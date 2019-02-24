function tryDeleteThing(parentSelector, obj, hideOthers = false) {
	var url = obj.attr("data-delete-request-url");
	var object = obj;
	object.prop("disabled", true);
	var loadingSpan = object.find(".spinner-border");
    loadingSpan.css("display", "inherit");
    if (hideOthers) {
	    loadingSpan.siblings().css("display", "none");
    }
	$.ajax({
		url: url,
		type: "DELETE",
        success: function (code, s) {
            object.closest(parentSelector).remove();
		},
		error: function () {
			loadingSpan.css("display", "none");
            object.prop("disabled", false);
            if (hideOthers) {
	            loadingSpan.siblings().css("display", ""); // remove
            }
		}
	});
}
$(document).ready(function () {
	$(document).on("click", "[data-review] [data-delete-request-url]", function () {
		tryDeleteThing("[data-review]", $(this));
    });
	$(document).on("click", ".idea [data-delete-request-url]", function () {
		tryDeleteThing(".idea-container", $(this), true);
	});
});
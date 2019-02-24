$(document).ready(function () {
    $(document).on("click", "button.vote", function () {
        if (!window.isSignedIn) {
            window.location.href = window.signInUrl;
            return;
        }

        var me = $(this);
        var url = me.attr("data-vote-url");
        var voteCount = me.siblings(".votes-count").first();
        var active = me.hasClass("active");
        var direction = me.attr("data-upvote") === undefined ? -1 : 1;

        if (active) direction = -direction;

        var evilBrother = me.siblings("button.vote"); // why? idk
        var brotherHadClass = evilBrother.hasClass("active");

        if (!active) {
            me.addClass("active");
            evilBrother.removeClass("active");
        } else {
	        me.removeClass("active");
        }
        var count = parseInt(voteCount.text());
        var modifiedDirection = direction * (brotherHadClass ? 2 : 1);
        count += modifiedDirection;
        voteCount.text(count.toString());

        $.ajax({
	        url: url,
	        type: "POST",
            error: function () {
                // oops let's revert
                me.removeClass("active");
                if (brotherHadClass) {
                    evilBrother.addClass("active");
                }
                count -= modifiedDirection;
                voteCount.text(count.toString());
            }
        });
    }); 
})
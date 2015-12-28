//UpdatePanel does provide cleanup hooks.
//When it's about to throw an element away, it checks to see if there is a element.dispose function. If such a function exists, then it will be executed.
(function($) {
	$.fn.Disposable = function(cln) {
		return this.each(function() {
			var el = this;
			if (!el.dispose) {
				el.dispose = cleanup;	// will be called by MS for cleanup
				$(window).bind("unload", cleanup);
			}
			if (cln) 
				(el.clnup = (el.clnup || [])).push(cln);
			function cleanup() {
				if (!el)
					return;
				if (el.clnup)
					for (var i = el.clnup.length - 1; i >= 0; i--)
						el.clnup[i]();
				$(el).unbind();
				$(window).unbind("unload", cleanup);
				el.clnup = el.dispose = null;
				el = null;
			};
		});
	};

	jQuery.fn.discard = function(fCleanOnly){
		jQuery( "*", this ).add([this]).each(function(){
			$(this).unbind();
			jQuery.removeData(this);
		});
		if (!fCleanOnly) {
			var bin = document.getElementById ('IELeakGarbageBin');
			if (!bin) {
				bin = document.createElement('DIV');
				bin.id = 'IELeakGarbageBin';
				document.body.appendChild(bin);
			}
			this.each(function(){
				bin.appendChild (this);
				bin.innerHTML = '';
			});
			bin = null;
		}
		return null;
	}

})(jQuery);

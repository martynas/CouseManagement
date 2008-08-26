/** Atrendia Course Management. */

/** Useful utility methods. */
String.extend({
    startsWith: function(prefix) {
        return this.substring(0, prefix.length) == prefix;
    },
    extractSuffix: function(prefix) {
        return this.startsWith(prefix)
               ? this.substring(prefix.length, this.length)
               : null;
    }    
});

Element.extend({
    /** Given prefix, extracts X if element has prefix-X as
     * a class. */
    extractClass: function(prefix) {
        var classes = this.className.split(/\s+/);
        prefix += "-";
        for (var i = 0; i < classes.length; i++) {
            var suffix = classes[i].extractSuffix(prefix);
            if (suffix) return suffix;
        }
        return null;
    },
    /** Given prefix, returns X is value is of form prefix-X. */
    extractSuffix: function(prefix, attribute) {
        return this.getProperty(attribute).extractSuffix(prefix + '-');
    },
    /** Hide element. */
    hide: function() {
        this.setStyle('display', 'none');
    },
    /** Show element; optional argument is style, defaults to ''. */
    show: function() {
        var value = arguments.length ? arguments[0] : '';
        this.setStyle('display', value);
    }
});

/** Magic links.  For every element (anchor) matched by selector, check if
 * its href is start for current path; if so, call apply with parent element
 * of element that has tag name of parentTag. */
function magicLinks(selector, parentTag, apply) {
    var current = window.location.pathname;
    $ES(selector).each(function (el) {
        var href = el.getProperty('href');
        parentTag = parentTag.toLowerCase();
        if (current.startsWith(href)) {
            var li = el.getParent();
            while (li && li.getTag() != parentTag) {
                li = li.getParent();
            }
            apply(li);
        }
    });
}

/** Table stripes. Odd/even colors for rows. */
function stripeTable(selector) {
    $ES(selector).each(function(table) {
       var rows = $ES('tr', table);
       var odd = true;
       for (var i = 0; i < rows.length; i++) {
           rows[i].addClass(odd ? 'odd' : 'even');
           odd = !odd;
       }
    });
}

/** Initialization. */
window.addEvent('domready', function() {
    magicLinks('#menu a', 'li', function(li) {
        li.addClass('active');
    });
    $ES('.focus-on-load').each(function(el) {
        el.focus();
    });
    $ES('input.select-all').each(function(el) {
        el.addEvent('click', function(ev) {
            ev = new Event(ev);
            var target = $(ev.target);
            var checked = target.checked;
            while (target != null && target.getTag() != 'table') {
                target = target.getParent();
            }
            if (target != null) {
                $ES('td input', target).each(function(input) {
                    if (input.getProperty('type').toLowerCase() == 'checkbox') {
                        input.checked = checked;
                    }
                });
            }
        });
    });
});

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
    $(selector).each(function (el) {
        var href = this.getAttribute('href');
        parentTag = parentTag.toLowerCase();
        if (current.startsWith(href)) {
            var li = this.parentNode;
            while (li && li.nodeName.toLowerCase() != parentTag) {
                li = li.parentNode;
            }
            apply(li);
        }
    });
}

/** Table stripes. Odd/even colors for rows. */
function stripeTable(selector) {
    $(selector).each(function(table) {
       var rows = ('tr', table);
       var odd = true;
       for (var i = 0; i < rows.length; i++) {
           rows[i].addClass(odd ? 'odd' : 'even');
           odd = !odd;
       }
    });
}

function getPreviousElementSibling(node){
    var e = node.previousSibling;
    while(e && e.nodeType !== 1)
        e = e.previousSibling;
    return e;
}

/** Initialization. */
window.addEvent('domready', function() {
    magicLinks('#menu a', 'li', function(li) {
        li.setAttribute("class", "active");
    });
    $('.focus-on-load').each(function(el) {
        this.focus();
    });
    $('input.select-all').each(function(el) {
        this.onclick = function(ev) {
        /*this.addEvent('click', function(ev) {*/
            /*ev = new Event(ev);*/
            var target = this;
            var checked = target.checked;
            while (target != null && target.nodeName.toLowerCase() != 'table') {
                target = target.parentNode;
            }
            if (target != null) {
                $('td input', target).each(function(input) {
                    if (this.getAttribute('type').toLowerCase() == 'checkbox') {
                        this.checked = checked;
                    }
                });
            }
        };
    });
    
    $('input.select-all2').each(function(el) {
        this.onclick = function(ev) {
        /*this.addEvent('click', function(ev) {*/
            ev = new Event(ev);
            var thobj = this;
            while (thobj != null && thobj.nodeName.toLowerCase() != 'th') {
                thobj = thobj.parentNode;
            }
            
            var i = 0;
            while (thobj != null && thobj.nodeName.toLowerCase() == 'th') {
                thobj = getPreviousElementSibling(thobj);
                i++;
            }
            var query = "tr td:nth-child(" + i + ") input:checkbox";
            
            var target = this;
            var checked = target.checked;
            while (target != null && target.nodeName.toLowerCase()!= 'table') {
                target = target.parentNode;
            }
            
            if (target != null) {
                $(query, target).each(function(input) {
                    this.checked = checked;
                });
            }
        };
    });
});

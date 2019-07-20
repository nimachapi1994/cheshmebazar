$(function () {
    $(".clickToShow").each(function (index) {
        var NUM = $(this).data('value');
        $($(this).data('target')).html(toTel(NUM));
        $(this).fadeOut('fast');
    });
    $('#categories').on('show.bs.dropdown', function () {
        $('#category-backdrop').fadeTo('fast', 0.5);
    }).on('shown.bs.dropdown', function () {
    }).on('hide.bs.dropdown', function () {
        $('#category-backdrop').fadeOut('fast');
    });
    $('#sidebar').on('click', function () {
        if ($('.main-row').hasClass('open')) {
            $('#category-backdrop').fadeOut('fast', function () {
                $('.main-row').removeClass('open');
            });
        } else {
            $('#category-backdrop').fadeTo('fast', 0.5, function () {
                $('.main-row').addClass('open');
            });
        }
        if (!$('.navbar-toggle').hasClass('collapsed'))
            $('.navbar-toggle').click();
    });
    $('.navbar-toggle').on('click', function () {
        if ($('.main-row').hasClass('open')) {
            $('#category-backdrop').fadeOut('fast', function () {
                $('.main-row').removeClass('open');
            });
        }
    });
    $('#category-backdrop').on('click', function () {
        $('#category-backdrop').fadeOut('fast', function () {
            $('.main-row').removeClass('open');
        });
    });
    $('[data-ui="select"]').each(function () {
        var $doneButton = $(this).data('done-button');
        $(this).selectpicker({
            doneButton: ($doneButton != null),
            doneButtonText: $doneButton,
            multipleSeparator: '٬ ',
            countSelectedText: function (numSelected, numTotal) {
                return (numSelected == 1) ? "{0} محدوده انتخاب شده" : "{0} محدوده انتخاب شده";
            },
            liveSearchPlaceholder: $(this).data('search-ph'),
            tickIcon: $(this).data('search-tick-icon'),
            iconBase: $(this).data('search-tick-base')
        });
    });
    $('.loading-layout')/*.delay(2000)*/.fadeOut(function () {
        if (typeof showLocationLayer !== 'undefined') {
            $('.location-select').css({ 'z-index': 1050 });
            $('#location-backdrop').fadeTo('fast', 0.5, function () {
                setTimeout(function () {
                    $('#location-backdrop').fadeOut();
                    $('.location-select').css({ 'z-index': 'auto' });
                }, 4000);
            });
        }
    });
    $('.scrollTo').each(function () {
        $(this).click(function () {
            var target = $(this).attr('href');
            var offset = $(this).data('offset') || 0;
            $('html, body').animate({
                scrollTop: $(target).offset().top + offset
            }, 2000);
            return false;
        });
    });

    $(".clickToShow").click(function (e) {
        var NUM = $(this).data('value');
        $($(this).data('target')).html(toTel(NUM));
        $(this).fadeOut('fast');
        e.preventDefault();

        return false;
    });
    $('#menu > li > a > span.icon').on('click', function (e) {
        $(this).parents('li').find('ul').slideToggle(500);
        e.preventDefault();
    });

    var groups = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Title'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Services/Misc/AutoCompleteForGroups?q=%QUERY',
            wildcard: '%QUERY'
        }
    });

    var ads = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Title'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Services/Misc/AutoCompleteForAds?q=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('.typeahead').typeahead({
        hint: false,
        updater: function (item) {
            window.location.href = item.Link;
        }
    }, {
        name: 'groups',
        display: 'Title',
        source: groups.ttAdapter(),
        matcher: function (item) { return true },
        templates: {
            header: '<h3 class="header-name">گروه های مرتبط</h3>',
            suggestion: function (data) {
                return '<a href="'+ data.Link +'">' + data.Title + '</a>';
            }
        },
        items: 4
    }, {
        name: 'ads',
        display: 'Title',
        source: ads.ttAdapter(),
        matcher: function (item) { return true },
        templates: {
            header: '<h3 class="header-name">مشاغل مرتبط</h3>',
            suggestion: function (data) {
                var a = '';
                if (data.Featured)
                    a = '<i class="fa fa-star fa-fw" style="color:gold;text-shadow:0px 0px 1px red"></i>'
                return '<a href="' + data.Link + '" data-featured="' + data.Featured + '">' + a + data.Title + '</a>';
            }
        },
        items: 4
    });

    $('form#recovery').submit(function () {
        var button = $(this).find('button');
        var $form = $(this);
        $.ajax({
            beforeSend: function () {
                button.button('loading');
            },
            success: function (data) {
                button.button('reset');
                if (data.statusCode == 0) {
                    $('#forgot-alert').removeClass('alert-warning alert-success').addClass('alert-warning').html('<i class="fa fa-exclamation-triangle fa-fw"></i> ' + data.statusMessage).show();
                } else if (data.statusCode == 1) {
                    $('#forgot-alert').removeClass('alert-warning alert-success').addClass('alert-success').html('<i class="fa fa-info-circle fa-fw"></i> ' + data.statusMessage).show();
                    $form.find("input[type=text], textarea").val("");
                }
            },
            error: function () {
                button.button('reset');
            },
            type: 'POST',
            url: $(this).attr('action'),
            data: $(this).serialize()
        });

        return false;
    });
});

function toTel(numbers) {
    numbers += "";
    var a = numbers.split('،');
    var b = [];
    for (var i = 0 ; i < a.length; i++) {
        var num = a[i].trim();
        var num2 = toPersianDigit(num);
        if (num.length <= 8 && !stringStartsWith(num, "021")) {
            num2 = "۰۲۱-" + num2;
            num = "021" + num;
        }
        b.push('<a href="tel://' + num + '" dir="ltr">' + num2 + "</a>");
    }

    return b.join(' ، ');
}

function toPersianDigit(str) {
    var dif = 1728;
    res = str;
    for (var c = '0'.charCodeAt(0) ; c <= '9'.charCodeAt(0) ; c++)
        res = res.split(String.fromCharCode(c)).join(String.fromCharCode(c + dif));
    return res;
}

function stringStartsWith(string, prefix) {
    return string.slice(0, prefix.length) == prefix;
}

function openMap(id) {
    var w = 900;
    var h = 500;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var targetWin = window.open('/Advertisement/Ads/MapPartial/' + id, 'map_' + id, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
    return targetWin;
}


$(window).scroll(function () {
    if ($(window).scrollTop() > 300 && screen.width > 500) {
        $('a.back-to-top').fadeIn('slow');
    } else {
        $('a.back-to-top').fadeOut('slow');
    }
});

$('a.back-to-top').click(function () {
    $('html, body').animate({
        scrollTop: 0
    }, 700);
    return false;
});

function getOsType() {
    if (navigator.userAgent.match(/Android/i))
        return 'Android';
    if (navigator.userAgent.match(/iPhone|iPad|iPod/i))
        return 'iOS';
    if (navigator.userAgent.match(/Mac/i))
        return 'Mac';
}
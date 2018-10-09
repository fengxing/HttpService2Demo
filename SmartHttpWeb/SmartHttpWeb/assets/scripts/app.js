/**
Core script to handle the entire theme and core functions
**/
var App = function () {
    var domain = "";//当前地址
    // IE mode
    var isRTL = false;
    var isIE8 = false;
    var isIE9 = false;
    var isIE10 = false;

    var sidebarWidth = 225;
    var sidebarCollapsedWidth = 35;

    var responsiveHandlers = [];

    // theme layout color set
    var layoutColorCodes = {
        'blue': '#4b8df8',
        'red': '#e02222',
        'green': '#35aa47',
        'purple': '#852b99',
        'grey': '#555555',
        'light-grey': '#fafafa',
        'yellow': '#ffb848'
    };

    /**
    *上传枚举类型,相册:0，楼书/特色/模板:1,资源文件:2,水印图3,专题4
    **/
    var UpLoadType = {
        Album: 0,
        File: 1,
        Resource: 2,
        WaterMarkPhoto: 3,
        Topic: 4,
        TopicImages: 5
    };


    // To get the correct viewport width based on  http://andylangton.co.uk/articles/javascript/get-viewport-size-javascript/
    var _getViewPort = function () {
        var e = window, a = 'inner';
        if (!('innerWidth' in window)) {
            a = 'client';
            e = document.documentElement || document.body;
        }
        return {
            width: e[a + 'Width'],
            height: e[a + 'Height']
        }
    }

    // initializes main settings
    var handleInit = function () {

        if ($('body').css('direction') === 'rtl') {
            isRTL = true;
        }

        isIE8 = !!navigator.userAgent.match(/MSIE 8.0/);
        isIE9 = !!navigator.userAgent.match(/MSIE 9.0/);
        isIE10 = !!navigator.userAgent.match(/MSIE 10.0/);

        if (isIE10) {
            jQuery('html').addClass('ie10'); // detect IE10 version
        }

        if (isIE10 || isIE9 || isIE8) {
            jQuery('html').addClass('ie'); // detect IE10 version
        }

        /*
          Virtual keyboards:
          Also, note that if you're using inputs in your modal – iOS has a rendering bug which doesn't 
          update the position of fixed elements when the virtual keyboard is triggered  
        */
        var deviceAgent = navigator.userAgent.toLowerCase();
        if (deviceAgent.match(/(iphone|ipod|ipad)/)) {
            $(document).on('focus', 'input, textarea', function () {
                $('.header').hide();
                $('.footer').hide();
            });
            $(document).on('blur', 'input, textarea', function () {
                $('.header').show();
                $('.footer').show();
            });
        }

    }

    var handleSidebarState = function () {
        // remove sidebar toggler if window width smaller than 992(for tablet and phone mode)
        var viewport = _getViewPort();
        if (viewport.width < 992) {
            $('body').removeClass("page-sidebar-closed");
        }
    }

    // runs callback functions set by App.addResponsiveHandler().
    var runResponsiveHandlers = function () {
        // reinitialize other subscribed elements
        for (var i in responsiveHandlers) {
            var each = responsiveHandlers[i];
            each.call();
        }
    }

    // reinitialize the laypot on window resize
    var handleResponsive = function () {
        handleSidebarState();
        handleSidebarAndContentHeight();
        handleFixedSidebar();
        runResponsiveHandlers();
    }

    // initialize the layout on page load
    var handleResponsiveOnInit = function () {
        handleSidebarState();
        handleSidebarAndContentHeight();
    }

    // handle the layout reinitialization on window resize
    var handleResponsiveOnResize = function () {
        var resize;
        if (isIE8) {
            var currheight;
            $(window).resize(function () {
                if (currheight == document.documentElement.clientHeight) {
                    return; //quite event since only body resized not window.
                }
                if (resize) {
                    clearTimeout(resize);
                }
                resize = setTimeout(function () {
                    handleResponsive();
                }, 50); // wait 50ms until window resize finishes.                
                currheight = document.documentElement.clientHeight; // store last body client height
            });
        } else {
            $(window).resize(function () {
                if (resize) {
                    clearTimeout(resize);
                }
                resize = setTimeout(function () {
                    handleResponsive();
                }, 50); // wait 50ms until window resize finishes.
            });
        }
    }

    //* BEGIN:CORE HANDLERS *//
    // this function handles responsive layout on screen size resize or mobile device rotate.

    // Set proper height for sidebar and content. The content and sidebar height must be synced always.
    var handleSidebarAndContentHeight = function () {
        var content = $('.page-content');
        var sidebar = $('.page-sidebar');
        var body = $('body');
        var height;

        if (body.hasClass("page-footer-fixed") === true && body.hasClass("page-sidebar-fixed") === false) {
            var available_height = $(window).height() - $('.footer').outerHeight();
            if (content.height() < available_height) {
                content.attr('style', 'min-height:' + available_height + 'px !important');
            }
        } else {
            if (body.hasClass('page-sidebar-fixed')) {
                height = _calculateFixedSidebarViewportHeight();
            } else {
                height = sidebar.height() + 20;
            }
            if (height >= content.height()) {
                content.attr('style', 'min-height:' + height + 'px !important');
            }
        }
    }

    // Handle sidebar menu
    var handleSidebarMenu = function () {
        jQuery('.page-sidebar').on('click', 'li > a', function (e) {
            if ($(this).next().hasClass('sub-menu') == false) {
                if ($('.btn-navbar').hasClass('collapsed') == false) {
                    $('.btn-navbar').click();
                }
                return;
            }

            if ($(this).next().hasClass('sub-menu.always-open')) {
                return;
            }

            var parent = $(this).parent().parent();
            var the = $(this);

            parent.children('li.open').children('a').children('.arrow').removeClass('open');
            parent.children('li.open').children('.sub-menu').slideUp(200);
            parent.children('li.open').removeClass('open');

            var sub = jQuery(this).next();
            var slideOffeset = -200;
            var slideSpeed = 200;

            if (sub.is(":visible")) {
                jQuery('.arrow', jQuery(this)).removeClass("open");
                jQuery(this).parent().removeClass("open");
                sub.slideUp(slideSpeed, function () {
                    if ($('body').hasClass('page-sidebar-fixed') == false && $('body').hasClass('page-sidebar-closed') == false) {
                        App.scrollTo(the, slideOffeset);
                    }
                    handleSidebarAndContentHeight();
                });
            } else {
                jQuery('.arrow', jQuery(this)).addClass("open");
                jQuery(this).parent().addClass("open");
                sub.slideDown(slideSpeed, function () {
                    if ($('body').hasClass('page-sidebar-fixed') == false && $('body').hasClass('page-sidebar-closed') == false) {
                        App.scrollTo(the, slideOffeset);
                    }
                    handleSidebarAndContentHeight();
                });
            }

            e.preventDefault();
        });

        // handle ajax links
        jQuery('.page-sidebar').on('click', ' li > a.ajaxify', function (e) {
            e.preventDefault();
            App.scrollTop();

            var url = $(this).attr("href");
            var menuContainer = jQuery('.page-sidebar ul');
            var pageContent = $('.page-content');
            var pageContentBody = $('.page-content .page-content-body');

            menuContainer.children('li.active').removeClass('active');
            menuContainer.children('arrow.open').removeClass('open');

            $(this).parents('li').each(function () {
                $(this).addClass('active');
                $(this).children('a > span.arrow').addClass('open');
            });
            $(this).parents('li').addClass('active');

            App.blockUI(pageContent, false);

            $.ajax({
                type: "GET",
                cache: false,
                url: url,
                dataType: "html",
                success: function (res) {
                    App.unblockUI(pageContent);
                    pageContentBody.html(res);
                    App.fixContentHeight(); // fix content height
                    App.initAjax(); // initialize core stuff
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    pageContentBody.html('<h4>Could not load the requested content.</h4>');
                    App.unblockUI(pageContent);
                },
                async: false
            });
        });
    }

    // Helper function to calculate sidebar height for fixed sidebar layout.
    var _calculateFixedSidebarViewportHeight = function () {
        var sidebarHeight = $(window).height() - $('.header').height() + 1;
        if ($('body').hasClass("page-footer-fixed")) {
            sidebarHeight = sidebarHeight - $('.footer').outerHeight();
        }

        return sidebarHeight;
    }

    // Handles fixed sidebar
    var handleFixedSidebar = function () {
        //var menu = $('.page-sidebar-menu');

        //if (menu.parent('.slimScrollDiv').size() === 1) { // destroy existing instance before updating the height
        //    menu.slimScroll({
        //        destroy: true
        //    });
        //    menu.removeAttr('style');
        //    $('.page-sidebar').removeAttr('style');
        //}

        //if ($('.page-sidebar-fixed').size() === 0) {
        //    handleSidebarAndContentHeight();
        //    return;
        //}

        //var viewport = _getViewPort();
        //if (viewport.width >= 992) {
        //    var sidebarHeight = _calculateFixedSidebarViewportHeight();

        //    menu.slimScroll({
        //        size: '7px',
        //        color: '#a1b2bd',
        //        opacity: .3,
        //        position: isRTL ? 'left' : 'right',
        //        height: sidebarHeight,
        //        allowPageScroll: false,
        //        disableFadeOut: false
        //    });
        //    handleSidebarAndContentHeight();
        //}
    }

    // Handles the sidebar menu hover effect for fixed sidebar.
    var handleFixedSidebarHoverable = function () {
        if ($('body').hasClass('page-sidebar-fixed') === false) {
            return;
        }

        $('.page-sidebar').off('mouseenter').on('mouseenter', function () {
            var body = $('body');

            if ((body.hasClass('page-sidebar-closed') === false || body.hasClass('page-sidebar-fixed') === false) || $(this).hasClass('page-sidebar-hovering')) {
                return;
            }

            body.removeClass('page-sidebar-closed').addClass('page-sidebar-hover-on');
            $(this).addClass('page-sidebar-hovering');
            $(this).animate({
                width: sidebarWidth
            }, 400, '', function () {
                $(this).removeClass('page-sidebar-hovering');
            });
        });

        $('.page-sidebar').off('mouseleave').on('mouseleave', function () {
            var body = $('body');

            if ((body.hasClass('page-sidebar-hover-on') === false || body.hasClass('page-sidebar-fixed') === false) || $(this).hasClass('page-sidebar-hovering')) {
                return;
            }

            $(this).addClass('page-sidebar-hovering');
            $(this).animate({
                width: sidebarCollapsedWidth
            }, 400, '', function () {
                $('body').addClass('page-sidebar-closed').removeClass('page-sidebar-hover-on');
                $(this).removeClass('page-sidebar-hovering');
            });
        });
    }

    // Handles sidebar toggler to close/hide the sidebar.
    var handleSidebarToggler = function () {
        var viewport = _getViewPort();
        if ($.cookie('sidebar_closed') === '1' && viewport.width >= 992) {
            $('body').addClass('page-sidebar-closed');
        }

        // handle sidebar show/hide
        $('.page-sidebar').on('click', '.sidebar-toggler', function (e) {
            var body = $('body');
            var sidebar = $('.page-sidebar');

            if ((body.hasClass("page-sidebar-hover-on") && body.hasClass('page-sidebar-fixed')) || sidebar.hasClass('page-sidebar-hovering')) {
                body.removeClass('page-sidebar-hover-on');
                sidebar.css('width', '').hide().show();
                $.cookie('sidebar_closed', '0');
                e.stopPropagation();
                runResponsiveHandlers();
                return;
            }

            $(".sidebar-search", sidebar).removeClass("open");

            if (body.hasClass("page-sidebar-closed")) {
                body.removeClass("page-sidebar-closed");
                if (body.hasClass('page-sidebar-fixed')) {
                    sidebar.css('width', '');
                }
                $.cookie('sidebar_closed', '0');
            } else {
                body.addClass("page-sidebar-closed");
                $.cookie('sidebar_closed', '1');
            }
            runResponsiveHandlers();
        });

        // handle the search bar close
        $('.page-sidebar').on('click', '.sidebar-search .remove', function (e) {
            e.preventDefault();
            $('.sidebar-search').removeClass("open");
        });

        // handle the search query submit on enter press
        $('.page-sidebar').on('keypress', '.sidebar-search input', function (e) {
            if (e.which == 13) {
                $('.sidebar-search').submit();
                return false; //<---- Add this line
            }
        });

        // handle the search submit
        $('.sidebar-search .submit').on('click', function (e) {
            e.preventDefault();
            if ($('body').hasClass("page-sidebar-closed")) {
                if ($('.sidebar-search').hasClass('open') == false) {
                    if ($('.page-sidebar-fixed').size() === 1) {
                        $('.page-sidebar .sidebar-toggler').click(); //trigger sidebar toggle button
                    }
                    $('.sidebar-search').addClass("open");
                } else {
                    $('.sidebar-search').submit();
                }
            } else {
                $('.sidebar-search').submit();
            }
        });
    }

    // Handles the horizontal menu
    var handleHorizontalMenu = function () {
        //handle hor menu search form toggler click
        $('.header').on('click', '.hor-menu .hor-menu-search-form-toggler', function (e) {
            if ($(this).hasClass('off')) {
                $(this).removeClass('off');
                $('.header .hor-menu .search-form').hide();
            } else {
                $(this).addClass('off');
                $('.header .hor-menu .search-form').show();
            }
            e.preventDefault();
        });

        //handle hor menu search button click
        $('.header').on('click', '.hor-menu .search-form .btn', function (e) {
            $('.form-search').submit();
            e.preventDefault();
        });

        //handle hor menu search form on enter press
        $('.header').on('keypress', '.hor-menu .search-form input', function (e) {
            if (e.which == 13) {
                $('.form-search').submit();
                return false;
            }
        });
    }

    // Handles the go to top button at the footer
    var handleGoTop = function () {
        /* set variables locally for increased performance */
        jQuery('.footer').on('click', '.go-top', function (e) {
            App.scrollTo();
            e.preventDefault();
        });
    }

    // Handles portlet tools & actions
    var handlePortletTools = function () {
        jQuery('body').on('click', '.portlet > .portlet-title > .tools > a.remove', function (e) {
            e.preventDefault();
            jQuery(this).closest(".portlet").remove();
        });

        jQuery('body').on('click', '.portlet > .portlet-title > .tools > a.reload', function (e) {
            e.preventDefault();
            var el = jQuery(this).closest(".portlet").children(".portlet-body");
            App.blockUI(el);
            window.setTimeout(function () {
                App.unblockUI(el);
            }, 1000);
        });

        jQuery('body').on('click', '.portlet > .portlet-title > .tools > .collapse, .portlet .portlet-title > .tools > .expand', function (e) {
            e.preventDefault();
            var el = jQuery(this).closest(".portlet").children(".portlet-body");
            if (jQuery(this).hasClass("collapse")) {
                jQuery(this).removeClass("collapse").addClass("expand");
                el.slideUp(200);
            } else {
                jQuery(this).removeClass("expand").addClass("collapse");
                el.slideDown(200);
            }
        });
    }

    // Handles custom checkboxes & radios using jQuery Uniform plugin
    var handleUniform = function () {
        if (!jQuery().uniform) {
            return;
        }
        var test = $("input[type=checkbox]:not(.toggle), input[type=radio]:not(.toggle, .star)");
        if (test.size() > 0) {
            test.each(function () {
                if ($(this).parents(".checker").size() == 0) {
                    $(this).show();
                    $(this).uniform();
                }
            });
        }
    }

    // Handles Bootstrap Accordions.
    var handleAccordions = function () {
        var lastClicked;
        //add scrollable class name if you need scrollable panes
        jQuery('body').on('click', '.accordion.scrollable .accordion-toggle', function () {
            lastClicked = jQuery(this);
        }); //move to faq section

        jQuery('body').on('show.bs.collapse', '.accordion.scrollable', function () {
            jQuery('html,body').animate({
                scrollTop: lastClicked.offset().top - 150
            }, 'slow');
        });
    }

    // Handles Bootstrap Tabs.
    var handleTabs = function () {
        // fix content height on tab click
        $('body').on('shown.bs.tab', '.nav.nav-tabs', function () {
            handleSidebarAndContentHeight();
        });

        //activate tab if tab id provided in the URL
        if (location.hash) {
            var tabid = location.hash.substr(1);
            $('a[href="#' + tabid + '"]').click();
        }
    }

    // Handles Bootstrap Modals.
    var handleModals = function () {

        // fix stackable modal issue: when 2 or more modals opened, closing one of modal will remove .modal-open class.
        // fix datarangpicker bug by lyh@20131031
        $('body').on('hidden.bs.modal', function () {
            if ($('.modal-dialog:visible').size() > 0 && $('html').hasClass('modal-open') == false) {
                $('html').addClass('modal-open');
            } else if ($('.modal-dialog:visible').size() == 0) {
                $('html').removeClass('modal-open');
            }
        });
    }

    // Handles Bootstrap Tooltips.
    var handleTooltips = function () {
        jQuery('.tooltips').tooltip({ html: true, container: $('body') });
    }

    // Handles Bootstrap Dropdowns
    var handleDropdowns = function () {
        /*
          For touch supported devices disable the 
          hoverable dropdowns - data-hover="dropdown"  
        */
        if (App.isTouchDevice()) {
            $('[data-hover="dropdown"]').each(function () {
                $(this).parent().off("hover");
                $(this).off("hover");
            });
        }
        /*
          Hold dropdown on click  
        */
        $('body').on('click', '.dropdown-menu.hold-on-click', function (e) {
            e.stopPropagation();
        })
    }

    // Handle Hower Dropdowns
    var handleDropdownHover = function () {
        $('[data-hover="dropdown"]').dropdownHover();
    }

    // Handles Bootstrap Popovers

    // last popep popover
    var lastPopedPopover;

    var handlePopovers = function () {
        jQuery('.popovers').popover();

        // close last poped popover

        $(document).on('click.bs.popover.data-api', function (e) {
            if (lastPopedPopover) {
                lastPopedPopover.popover('hide');
            }
        });
    }

    // Handles scrollable contents using jQuery SlimScroll plugin.
    var handleScrollers = function () {
        //$('.scroller').each(function () {
        //    var height;
        //    if ($(this).attr("data-height")) {
        //        height = $(this).attr("data-height");
        //    } else {
        //        height = $(this).css('height');
        //    }
        //    $(this).slimScroll({
        //        size: '7px',
        //        color: '#a1b2bd',
        //        position: isRTL ? 'left' : 'right',
        //        height: height,
        //        alwaysVisible: ($(this).attr("data-always-visible") == "1" ? true : false),
        //        railVisible: ($(this).attr("data-rail-visible") == "1" ? true : false),
        //        disableFadeOut: true
        //    });
        //});
    }

    // Handles Image Preview using jQuery Fancybox plugin
    var handleFancybox = function () {
        if (!jQuery.fancybox) {
            return;
        }

        if (jQuery(".fancybox-button").size() > 0) {
            jQuery(".fancybox-button").fancybox({
                groupAttr: 'data-rel',
                prevEffect: 'none',
                nextEffect: 'none',
                closeBtn: true,
                helpers: {
                    title: {
                        type: 'inside'
                    }
                }
            });
        }
    }

    // Fix input placeholder issue for IE8 and IE9
    var handleFixInputPlaceholderForIE = function () {
        //fix html5 placeholder attribute for ie7 & ie8
        if (isIE8 || isIE9) { // ie8 & ie9
            // this is html5 placeholder fix for inputs, inputs with placeholder-no-fix class will be skipped(e.g: we need this for password fields)
            jQuery('input[placeholder]:not(.placeholder-no-fix), textarea[placeholder]:not(.placeholder-no-fix)').each(function () {

                var input = jQuery(this);

                if (input.val() == '' && input.attr("placeholder") != '') {
                    input.addClass("placeholder").val(input.attr('placeholder'));
                }

                input.focus(function () {
                    if (input.val() == input.attr('placeholder')) {
                        input.val('');
                    }
                });

                input.blur(function () {
                    if (input.val() == '' || input.val() == input.attr('placeholder')) {
                        input.val(input.attr('placeholder'));
                    }
                });
            });
        }
    }

    // Handle full screen mode toggle
    var handleFullScreenMode = function () {
        // mozfullscreenerror event handler

        // toggle full screen
        function toggleFullScreen() {
            if (!document.fullscreenElement && // alternative standard method
                !document.mozFullScreenElement && !document.webkitFullscreenElement) { // current working methods
                if (document.documentElement.requestFullscreen) {
                    document.documentElement.requestFullscreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullscreen) {
                    document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (document.cancelFullScreen) {
                    document.cancelFullScreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                }
            }
        }

        $('#trigger_fullscreen').click(function () {
            toggleFullScreen();
        });
    }

    // Handle Select2 Dropdowns
    var handleSelect2 = function () {
        if (jQuery().select2) {
            $('.select2me').select2({
                placeholder: "请选择",
                allowClear: true
            });
        }
    }

    // Handle Theme Settings
    var handleTheme = function () {

        var panel = $('.theme-panel');

        if ($('body').hasClass('page-boxed') == false) {
            $('.layout-option', panel).val("fluid");
        }

        $('.sidebar-option', panel).val("default");
        $('.header-option', panel).val("fixed");
        $('.footer-option', panel).val("default");

        //handle theme layout
        var resetLayout = function () {
            $("body").
            removeClass("page-boxed").
            removeClass("page-footer-fixed").
            removeClass("page-sidebar-fixed").
            removeClass("page-header-fixed");

            $('.header > .header-inner').removeClass("container");

            if ($('.page-container').parent(".container").size() === 1) {
                $('.page-container').insertAfter('body > .clearfix');
            }

            if ($('.footer > .container').size() === 1) {
                $('.footer').html($('.footer > .container').html());
            } else if ($('.footer').parent(".container").size() === 1) {
                $('.footer').insertAfter('.page-container');
            }

            $('body > .container').remove();
        }

        var lastSelectedLayout = '';

        var setLayout = function () {

            var layoutOption = $('.layout-option', panel).val();
            var sidebarOption = $('.sidebar-option', panel).val();
            var headerOption = $('.header-option', panel).val();
            var footerOption = $('.footer-option', panel).val();

            if (sidebarOption == "fixed" && headerOption == "default") {
                alert('Default Header with Fixed Sidebar option is not supported. Proceed with Fixed Header with Fixed Sidebar.');
                $('.header-option', panel).val("fixed");
                $('.sidebar-option', panel).val("fixed");
                sidebarOption = 'fixed';
                headerOption = 'fixed';
            }

            resetLayout(); // reset layout to default state

            if (layoutOption === "boxed") {
                $("body").addClass("page-boxed");

                // set header
                $('.header > .header-inner').addClass("container");
                var cont = $('body > .clearfix').after('<div class="container"></div>');

                // set content
                $('.page-container').appendTo('body > .container');

                // set footer
                if (footerOption === 'fixed') {
                    $('.footer').html('<div class="container">' + $('.footer').html() + '</div>');
                } else {
                    $('.footer').appendTo('body > .container');
                }
            }

            if (lastSelectedLayout != layoutOption) {
                //layout changed, run responsive handler:
                runResponsiveHandlers();
            }
            lastSelectedLayout = layoutOption;

            //header
            if (headerOption === 'fixed') {
                $("body").addClass("page-header-fixed");
                $(".header").removeClass("navbar-static-top").addClass("navbar-fixed-top");
            } else {
                $("body").removeClass("page-header-fixed");
                $(".header").removeClass("navbar-fixed-top").addClass("navbar-static-top");
            }

            //sidebar
            if (sidebarOption === 'fixed') {
                $("body").addClass("page-sidebar-fixed");
            } else {
                $("body").removeClass("page-sidebar-fixed");
            }

            //footer 
            if (footerOption === 'fixed') {
                $("body").addClass("page-footer-fixed");
            } else {
                $("body").removeClass("page-footer-fixed");
            }

            handleSidebarAndContentHeight(); // fix content height            
            handleFixedSidebar(); // reinitialize fixed sidebar
            handleFixedSidebarHoverable(); // reinitialize fixed sidebar hover effect
        }

        // handle theme colors
        var setColor = function (color) {
            $('#style_color').attr("href", "/assets/css/themes/" + color + ".css");
            $.cookie('style_color', color);
        }


        //
        $('.toggler', panel).click(function () {
            $('.toggler').hide();
            $('.toggler-close').show();
            $('.theme-panel > .theme-options').show();
        });

        $('.toggler-close', panel).click(function () {
            $('.toggler').show();
            $('.toggler-close').hide();
            $('.theme-panel > .theme-options').hide();
        });

        $('.theme-colors > ul > li', panel).click(function () {
            var color = $(this).attr("data-style");
            setColor(color);
            $('ul > li', panel).removeClass("current");
            $(this).addClass("current");
        });

        $('.layout-option, .header-option, .sidebar-option, .footer-option', panel).change(setLayout);

        if ($.cookie('style_color')) {
            setColor($.cookie('style_color'));
        }
    }

    //* END:CORE HANDLERS *//

    return {

        //main function to initiate the theme
        init: function () {

            //IMPORTANT!!!: Do not modify the core handlers call order.

            //core handlers
            handleInit(); // initialize core variables
            handleResponsiveOnResize(); // set and handle responsive    
            handleUniform(); // hanfle custom radio & checkboxes
            handleScrollers(); // handles slim scrolling contents 
            handleResponsiveOnInit(); // handler responsive elements on page load

            //layout handlers
            handleFixedSidebar(); // handles fixed sidebar menu
            handleFixedSidebarHoverable(); // handles fixed sidebar on hover effect 
            handleSidebarMenu(); // handles main menu
            handleHorizontalMenu(); // handles horizontal menu
            handleSidebarToggler(); // handles sidebar hide/show            
            //handleFixInputPlaceholderForIE(); // fixes/enables html5 placeholder attribute for IE9, IE8
            handleGoTop(); //handles scroll to top functionality in the footer
            handleTheme(); // handles style customer tool

            //ui component handlers
            handleFancybox() // handle fancy box
            handleSelect2(); // handle custom Select2 dropdowns
            handlePortletTools(); // handles portlet action bar functionality(refresh, configure, toggle, remove)
            handleDropdowns(); // handle dropdowns
            handleTabs(); // handle tabs
            handleTooltips(); // handle bootstrap tooltips
            handlePopovers(); // handles bootstrap popovers
            handleAccordions(); //handles accordions 
            handleModals(); // handle modals
            handleFullScreenMode() // handles full screen
        },

        //main function to initiate core javascript after ajax complete
        initAjax: function () {
            handleSelect2(); // handle custom Select2 dropdowns
            handlePortletTools(); // handles portlet action bar functionality(refresh, configure, toggle, remove)
            handleDropdowns(); // handle dropdowns
            handleTooltips(); // handle bootstrap tooltips
            handlePopovers(); // handles bootstrap popovers
            handleAccordions(); //handles accordions 
            handleUniform(); // hanfle custom radio & checkboxes     
            handleDropdownHover() // handles dropdown hover       
        },

        //public function to fix the sidebar and content height accordingly
        fixContentHeight: function () {
            handleSidebarAndContentHeight();
        },

        //public function to remember last opened popover that needs to be closed on click
        setLastPopedPopover: function (el) {
            lastPopedPopover = el;
        },

        //public function to add callback a function which will be called on window resize
        addResponsiveHandler: function (func) {
            responsiveHandlers.push(func);
        },

        // useful function to make equal height for contacts stand side by side
        setEqualHeight: function (els) {
            var tallestEl = 0;
            els = jQuery(els);
            els.each(function () {
                var currentHeight = $(this).height();
                if (currentHeight > tallestEl) {
                    tallestColumn = currentHeight;
                }
            });
            els.height(tallestEl);
        },

        // wrapper function to scroll(focus) to an element
        scrollTo: function (el, offeset) {
            //pos = (el && el.size() > 0) ? el.offset().top : 0;
            //jQuery('html,body').animate({
            //    scrollTop: pos + (offeset ? offeset : 0)
            //}, 'slow');
        },

        // function to scroll to the top
        scrollTop: function () {
            App.scrollTo();
        },

        // wrapper function to  block element(indicate loading)
        blockUI: function (el, centerY) {
            var el = jQuery(el);
            if (el.height() <= 400) {
                centerY = true;
            }
            el.block({
                message: '<img src="/assets/img/ajax-loading.gif" align="">',
                centerY: centerY != undefined ? centerY : true,
                css: {
                    top: '10%',
                    border: 'none',
                    padding: '2px',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: '#000',
                    opacity: 0.05,
                    cursor: 'wait'
                }
            });
        },

        // wrapper function to  un-block element(finish loading)
        unblockUI: function (el) {
            jQuery(el).unblock({
                onUnblock: function () {
                    jQuery(el).removeAttr("style");
                }
            });
        },

        // initializes uniform elements
        initUniform: function (els) {
            if (els) {
                jQuery(els).each(function () {
                    if ($(this).parents(".checker").size() == 0) {
                        $(this).show();
                        $(this).uniform();
                    }
                });
            } else {
                handleUniform();
            }

        },

        handleTooltips: function () {
            handleTooltips();
        },

        //wrapper function to update/sync jquery uniform checkbox & radios
        updateUniform: function (els) {
            $.uniform.update(els); // update the uniform checkbox & radios UI after the actual input control state changed
        },

        //public function to initialize the fancybox plugin
        initFancybox: function () {
            handleFancybox();
        },

        //public helper function to get actual input value(used in IE9 and IE8 due to placeholder attribute not supported)
        getActualVal: function (el) {
            var el = jQuery(el);
            if (el.val() === el.attr("placeholder")) {
                return "";
            }
            return el.val();
        },

        //public function to get a paremeter by name from URL
        getURLParameter: function (paramName) {
            var searchString = window.location.search.substring(1),
                i, val, params = searchString.split("&");

            for (i = 0; i < params.length; i++) {
                val = params[i].split("=");
                if (val[0] == paramName) {
                    return unescape(val[1]);
                }
            }
            return null;
        },

        // check for device touch support
        isTouchDevice: function () {
            try {
                document.createEvent("TouchEvent");
                return true;
            } catch (e) {
                return false;
            }
        },

        // check IE8 mode
        isIE8: function () {
            return isIE8;
        },

        // check IE9 mode
        isIE9: function () {
            return isIE9;
        },

        //check RTL mode
        isRTL: function () {
            return isRTL;
        },

        // get layout color code by color name
        getLayoutColorCode: function (name) {
            if (layoutColorCodes[name]) {
                return layoutColorCodes[name];
            } else {
                return '';
            }
        },

        /****
        *上传地址
        ****/
        //2015-04图片上传地址整合  （在Layout中会进行赋值）
        //UploadBaseURL: "http://upload.mallcoo.cn/",
        //ImgBaseURL: "http://i1.mallcoo.cn/",
        UploadBaseURL: "",
        ImgBaseURL: "",
        PageSize: 10,

        PageSize100: 100,

        /****
        *上传枚举类型,相册:0，楼书/特色/模板:1,资源文件:2,水印图3,专题4
        ****/
        UpLoadType: {
            Album: 0,
            File: 1,
            Resource: 2,
            WaterMarkPhoto: 3,
            Topic: 4,
            TopicImages: 5
        },

        /****
         * Uploadify
        ****/
        uploadify: function (fileID, btnImg, btnW, btnH, folder, scriptData, onComplete) {
            var _uploadify = $('#' + fileID).uploadify({
                'uploader': '/assets/plugins/jquery-uploadify/uploadify.swf',
                'script': App.UploadBaseURL,
                'scriptData': { 'UpLoadFileType': scriptData },
                'folder': folder,
                'buttonImg': btnImg,
                'rollover': true,
                'cancelImg': '/assets/plugins/jquery-uploadify/uploadify-cancel.png',
                'width': btnW,
                'height': btnH,
                'scriptAccess': 'always',
                'auto': true,
                'multi': false,
                'fileExt': '*.jpg;*.gif;*.png;*.jpeg',
                'fileDesc': 'Web Files (*.jpg;*.gif;*.png;*.jpeg)',
                'onOpen': function (event, queueId, fileObj) {
                },
                'onSelect': function () {
                },
                'onError': function () {
                    if (arguments[3] != null) {
                        window.alert('上传错误请重试，代码：' + arguments[3].info);
                    }
                },
                'onComplete': onComplete
            });
        },

        /****
         * Uploadifive(html5版本)
        ****/
        uploadifive: function (fileID, btnText, btnW, btnH, queueID, folder, fileType, onComplete) {

            if (App.getBrowserVersion() < 10) {
                if (App.hasFlashPlayer()) {
                    //App.uploadify(fileID, "http://i0.mallcoo.cn/images/uploadfile.png", 90, 20, folder, fileType, onComplete);
                    App.uploadify(fileID, App.ImgBaseURL + "mc/adf0dd48-a7a4-4d80-8080-d98456d8e8aa.png", 90, 20, folder, fileType, onComplete);
                } else {
                    alert("检测到浏览器未安装或已禁用Flash插件，请安装或启用后重新访问该页面。");
                }
            } else {

                var _uploadify = $('#' + fileID).uploadifive({
                    'auto': true,
                    'multi': false,
                    'buttonText': btnText,
                    'buttonClass': 'button-size',//自定义样式
                    'width': btnW,
                    'height': btnH,
                    'queueID': queueID,
                    'removeCompleted': true,
                    'formData': { 'folder': folder, 'UpLoadFileType': fileType },
                    'uploadScript': App.UploadBaseURL,
                    'onSelect': function () {
                    },
                    'onError': function () {
                        if (arguments[3] != null) {
                            window.alert('上传错误请重试，代码：' + arguments[3].info);
                        }
                    },
                    'onUploadComplete': function (file, data) {
                        onComplete(null, null, file, data);
                    }
                });
            }
        },


        /****
        * shwo tips
       ****/
        showTips: function (msg, shwoTime, callback) {
            shwoTime = shwoTime || 2000;
            $('#modal_tip_body').html(msg);
            $('#modal_tip').modal('show');
            setTimeout(function () {
                $('#modal_tip').modal('hide');
                if (callback) {
                    callback();
                }
            }, shwoTime);
        },

        /****
        * 是否价格
        ****/
        isPrice: function (value) {
            var tPrice = /^[0-9]+(.[0-9]{1,2})?$/;
            if (value != undefined && tPrice.test(value)) {
                return true;
            }
            else return false;
        },

        /**
        * Init AutoComplete Control
        * @param {String} elemID
        * @param {String} url
        * @param {Function} onSelect
        * @param {JSONObject} params
        */
        initAutoComplete: function (elemID, url, onSelect, params) {
            if ($('#' + elemID)[0] != null) {
                $('#' + elemID).autocomplete({
                    deferRequestBy: 500,
                    params: params,
                    delimiter: true,
                    serviceUrl: url,
                    noCache: true,
                    onSelect: onSelect
                });
            }
        },

        //处理图片路径(WidthBase)
        getNewImage: function (path, width, height) {
            var url = path.slice(path.indexOf('/'));
            var pathArray = url.replace(/\//g, "").split('.');
            if (pathArray.length > 1) {
                if (!width || !height) {
                    return pathArray[0] + "_50x50_1." + pathArray[1];
                }
                else {
                    return pathArray[0] + "_" + width + "x" + height + "_3_0_0." + pathArray[1];
                }
            }
            return null;
        },

        /**
         *图片自定义类型枚举(0:等比缩放;1:裁剪;2:补白;3:以宽度为准进行等比缩放;4:以高度为准进行等比缩放)
        **/
        PhotoResizeType: {
            Scaling: 0,
            Cut: 1,
            FillUp: 2,
            WidthBase: 3,
            HeightBase: 4
        },

        //处理图片路径(WidthBase)
        getNewImageByType: function (path, width, height, photoResizeType) {
            var url = path.slice(path.indexOf('/'));
            var pathArray = url.replace(/\//g, "").split('.');
            if (pathArray.length > 1) {
                if (!width || !height) {
                    return pathArray[0] + "_50x50_1." + pathArray[1];
                }
                else {
                    return pathArray[0] + "_" + width + "x" + height + "_" + photoResizeType + "_0_0." + pathArray[1];
                }
            }
            return null;
        },

        //省市
        getCities: function () {
            return App.getSelectorJSONArray(CityArray);
        },

        //区
        getDistricts: function (cityVal) {
            if (cityVal == null || cityVal == '') {
                return App.getSelectorJSONArray();
            }
            else {
                var _districtArray = [];
                for (i in District) {
                    if (i.substring(0, 2) == cityVal) {
                        _districtArray[i] = District[i];
                    }
                }
                return App.getSelectorJSONArray(_districtArray);
            }
        },
        //根据城市名称获取区县列表
        getDistrictsByCityName: function (cityTxt) {
            if (cityTxt == null || cityTxt == '') {
                return App.getSelectorJSONArray();
            }
            else {
                var cityVal = "";
                for (i in CityArray) {
                    if (CityArray[i] == cityTxt) {
                        cityVal = i;
                    }
                }

                return App.getDistricts(cityVal);
            }
        },

        //获取拼音
        /**
        *name:中文名称
        *targetElem:拼音输入框
        **/
        getPinYin: function (name, targetElem) {
            if (name == null || name == "") {
                App.showTips("请先输入名称");
                return false;
            }
            $.ajax({
                type: 'POST',
                url: "/Service/GetPinYin",
                data: "name=" + encodeURIComponent(name),
                error: function (_error) {
                    App.showTips("获取拼音失败");
                },
                success: function (res) {
                    $("#" + targetElem).val(res);
                }
            });
        },

        /**
         * Get Selector Control JSONArray DataSource
         * @param {Array} array
         * @param {String} defTxt
         * @param {String} defVal
         * @return {JSONArray}
         */
        getSelectorJSONArray: function (array, defTxt, defVal) {
            if (defTxt == null) {
                defTxt = "请选择";
            }
            if (defVal == null) {
                defVal = "";
            }
            var JSONArray = [];

            if (array != null) {
                for (i in array) {
                    JSONArray.push({ txt: array[i], val: i });
                }
            }
            var exist = false;
            $.each(JSONArray, function (i) {
                if (this.txt == defTxt && this.val == defVal) {
                    exist = true;
                }
            });
            if (!exist) {
                JSONArray.unshift({ txt: defTxt, val: defVal });
            }
            return JSONArray;
        },

        //比较两个时间大小 日期格式：2012-12-12 12:12:12
        compareDateTime: function (startDate, endDate) {
            var reg = new RegExp("-", "g");
            var startTime = Date.parse(startDate.replace(reg, "/"));
            var endTime = Date.parse(endDate.replace(reg, "/"));

            return endTime < startTime;
        },

        //验证身份证
        validateIdCard: function (obj) {
            //是否验证空值
            //不验证空值
            if (!obj) {
                return true;
            }
            var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙 江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖 北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西 藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国 外" };
            var iSum = 0;
            //var info = "";
            var strIDno = obj;
            var idCardLength = strIDno.length;
            if (!/^\d{17}(\d|x)$/i.test(strIDno) && !/^\d{15}$/i.test(strIDno))
                return false; //非法身份证号

            if (aCity[parseInt(strIDno.substr(0, 2))] == null)
                return false;// 非法地区

            // 15位身份证转换为18位
            if (idCardLength == 15) {
                sBirthday = "19" + strIDno.substr(6, 2) + "-" + Number(strIDno.substr(8, 2)) + "-" + Number(strIDno.substr(10, 2));
                var d = new Date(sBirthday.replace(/-/g, "/"))
                var dd = d.getFullYear().toString() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
                if (sBirthday != dd)
                    return 3; //非法生日
                strIDno = strIDno.substring(0, 6) + "19" + strIDno.substring(6, 15);
                strIDno = strIDno + GetVerifyBit(strIDno);
            }

            // 判断是否大于2078年，小于1900年
            var year = strIDno.substring(6, 10);
            if (year < 1900 || year > 2078)
                return false;//非法生日

            //18位身份证处理

            //在后面的运算中x相当于数字10,所以转换成a
            strIDno = strIDno.replace(/x$/i, "a");

            sBirthday = strIDno.substr(6, 4) + "-" + Number(strIDno.substr(10, 2)) + "-" + Number(strIDno.substr(12, 2));
            var d = new Date(sBirthday.replace(/-/g, "/"))
            if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate()))
                return false; //非法生日
            // 身份证编码规范验证
            for (var i = 17; i >= 0; i--)
                iSum += (Math.pow(2, i) % 11) * parseInt(strIDno.charAt(17 - i), 11);
            if (iSum % 11 != 1)
                return false;// 非法身份证号

            // 判断是否屏蔽身份证
            var words = new Array();
            words = new Array("11111119111111111", "12121219121212121");

            for (var k = 0; k < words.length; k++) {
                if (strIDno.indexOf(words[k]) != -1) {
                    return false;
                }
            }

            return true;
        },
        //转义HTML
        htmlDecode: function (str) {
            var s = "";
            if (str.length == 0) return "";
            s = str.replace(/&/g, "&amp;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
            s = s.replace(/    /g, "&nbsp;");
            s = s.replace(/\'/g, "'");
            s = s.replace(/\"/g, "&quot;");
            s = s.replace(/\n/g, "<br>");
            return s;
        },
        //获取浏览器版本
        getBrowserVersion: function () {
            var ver = 100,
            ie = (function () {
                var undef,
                    v = 3,
                    div = document.createElement('div'),
                    all = div.getElementsByTagName('i');
                while (
                    div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
                    all[0]
                );
                return v > 4 ? v : undef;
            }());
            if (ie) ver = ie;
            return ver;
        },

        hasFlashPlayer: function () {
            //navigator.mimeTypes是MIME类型，包含插件信息
            if (navigator.mimeTypes.length > 0) {
                //application/x-shockwave-flash是flash插件的名字
                var flashAct = navigator.mimeTypes["application/x-shockwave-flash"];
                return flashAct != null ? flashAct.enabledPlugin != null : false;
            } else if (self.ActiveXObject) {
                try {
                    new ActiveXObject('ShockwaveFlash.ShockwaveFlash');
                    return true;
                } catch (oError) {
                    return false;
                }
            }
        },

        //获取原始图片尺寸
        imgLoad: function (url, callback) {
            var img = new Image();
            img.src = url;

            if (img.complete) {
                callback(img.width, img.height);
            } else {
                img.onload = function () {
                    callback(img.width, img.height);
                    img.onload = null;
                }
            }
        }


    };

}();

/**
* Serialize Form Elements To JSON
*/
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name] += (',' + ($.trim(this.value) || ''));     // CheckBox Value Split with ','
            //o[this.name].push($.trim(this.value) || '');
        } else {
            o[this.name] = $.trim(this.value) || '';
        }
    });
    return o;
};

$.validator.addMethod('mobile', function (value, element) {
    var length = value.length;
    //return this.optional(element) || (length == 11 && /(^1[35678][0-9]{9}$)/.test(value));
    return this.optional(element) || (length == 11 && /(^[0-9]{11}$)/.test(value));
}, '请输入格式正确的手机号码');

$.validator.addMethod('isIDNum', function (value, element) { if (App.validateIdCard(value)) { return true; } return false; }, '无效的身份证号');//true

$.validator.addMethod('checkImg', function (value, element) { return !(/(src="data:image\/)/gi.test(value)); }, '请上传图片，不要直接粘贴图片');//!false

$.validator.addMethod('checkPrice2', function (value, element) { return (/^[0-9]+(\.([1-9]|\d[0-9]))?$/.test(value)); }, '价格只能输入2位小数');//true

/* 得到日期年月日等加数字后的日期 */
Date.prototype.dateAdd = function (interval, number) {
    var d = this;
    var k = { 'y': 'FullYear', 'q': 'Month', 'm': 'Month', 'w': 'Date', 'd': 'Date', 'h': 'Hours', 'n': 'Minutes', 's': 'Seconds', 'ms': 'MilliSeconds' };
    var n = { 'q': 3, 'w': 7 };
    eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
    return d;
}

/* 计算两日期相差的日期年月日等 */
Date.prototype.dateDiff = function (interval, objDate2) {
    var d = this, i = {}, t = d.getTime(), t2 = objDate2.getTime();
    i['y'] = objDate2.getFullYear() - d.getFullYear();
    i['q'] = i['y'] * 4 + Math.floor(objDate2.getMonth() / 4) - Math.floor(d.getMonth() / 4);
    i['m'] = i['y'] * 12 + objDate2.getMonth() - d.getMonth();
    i['ms'] = objDate2.getTime() - d.getTime();
    i['w'] = Math.floor((t2 + 345600000) / (604800000)) - Math.floor((t + 345600000) / (604800000));
    i['d'] = Math.floor(t2 / 86400000) - Math.floor(t / 86400000);
    i['h'] = Math.floor(t2 / 3600000) - Math.floor(t / 3600000);
    i['n'] = Math.floor(t2 / 60000) - Math.floor(t / 60000);
    i['s'] = Math.floor(t2 / 1000) - Math.floor(t / 1000);
    return i[interval];
}

/* 获取字符串字节数长度 */
String.prototype.getBytesLength = function () {
    var str = this;
    var l = 0;
    for (var ii = 0; ii < str.length; ii++) {
        var word = str.substring(ii, 1);
        if (/[^\x00-\xff]/g.test(word)) {
            l += 2;
        } else {
            l++;
        }
    }
    return l;
}


/* 将json字符串转换为日期对象，如 /Date(1239018869048)/ 
       var s = "Date(1239018869048)";
       alert(new Date().parseDateFromJson(s));
*/
Date.prototype.parseDateFromJson = function (dateStr) {
    var str = dateStr.substring(6, dateStr.length - 2);
    return new Date().dateFormatter(new Date(str * 1), 103);
};

Date.prototype.dateFormatter = function (now, type) {
    var autoDates = "";
    var year = now.getFullYear();
    var month = now.getMonth();
    var day = now.getDate();
    var hour = now.getHours();
    var miunte = now.getMinutes();
    var second = now.getSeconds();
    //
    month = month < 9 ? "0" + (month + 1) : (month + 1);
    day = day < 10 ? "0" + day : day;
    hour = hour < 10 ? "0" + hour : hour;
    miunte = miunte < 10 ? "0" + miunte : miunte;
    second = second < 10 ? "0" + second : second;
    //
    if (type == 100)
        autoDates = year + "-" + month + "-" + day;
    if (type == 101)
        autoDates = year + "-" + month + "-" + day + " " + hour;
    if (type == 102)
        autoDates = year + "-" + month + "-" + day + " " + hour + ":" + miunte;
    if (type == 103)
        autoDates = year + "-" + month + "-" + day + " " + hour + ":" + miunte + ":" + second;
    if (year < 100)
        autoDates = "——";

    return autoDates;
}

/*
    fixedTable 
    固定表格右列
*/

var fixedTableColumn = function (ftable, fwrap, frnum) {

    //修复dropdownmenu
    dropdown = $('.fixed_table > .dropdown-menu');

    dropdown.parent().on('shown.bs.dropdown', function () {
        var $menu = $("ul", this);
        width = $menu.width();
        offset = $menu.offset();
        position = $menu.position();
        $('body').append($menu);
        $menu.show();
        $menu.css('position', 'absolute');
        $menu.css('top', (offset.top) + 'px');
        $menu.css('left', (offset.left) + 'px');
        $menu.css('width', (width) + 'px');
        $(this).data("myDropdownMenu", $menu);
    });
    dropdown.parent().on('hide.bs.dropdown', function () {
        $(this).append($(this).data("myDropdownMenu"));
        $(this).data("myDropdownMenu").removeAttr('style');
    });
    //end

    if (!ftable)
        ftable = $('.fixed_table');
    var ftobj = $('.fixed_table tbody tr td').length;
    if (!fwrap)
        fwrap = $('.tableinner');
    if (!frnum)
        frnum = '1';

    var tw = ftable.width();
    var pw = fwrap.width();

    //无视table和外部div大小判定，都执行DataTable
    if (ftable.length > 0 && ftobj > 1 /*&& tw > pw*/) {
        //fixedtable
        var tableFix = $('.fixed_table').DataTable({
            scrollX: true,
            destroy: true, //added by wangjianglin 2016/3/9
            paging: false,
            fixedHeader: false,
            bPaginate: false, //翻页功能
            bLengthChange: false, //改变每页显示数据数量
            bFilter: false, //过滤功能
            bSort: false, //排序功能
            bInfo: false,//页脚信息
            bAutoWidth: true,
            fixedColumns: {
                rightColumns: frnum,
                leftColumns: 0
            },
            oLanguage: {
                sEmptyTable: "暂无数据"
            }
        });

        //当屏幕大小变化时，重新调整table的
        $(window).resize(function () {
            // 重新绘制表格
			$('#DataTables_Table_0 tr').css('height','auto'); //2016/8/16 kmkim add
            tableFix.draw();
        });

    }

}

//修复dropdownmenu被截断的bug
var dropDownFixPosition = function (dropdown) {

    if (!dropdown)
        dropdown = $('.table .dropdown-menu');

    dropdown.parent().on('shown.bs.dropdown', function () {
        var $menu = $("ul", this);
        width = $menu.width();
        offset = $menu.offset();
        position = $menu.position();
        $('body').append($menu);
        $menu.show();
        $menu.css('position', 'absolute');
        $menu.css('top', (offset.top) + 'px');
        $menu.css('left', (offset.left) + 'px');
        $menu.css('width', (width) + 'px');
        $(this).data("myDropdownMenu", $menu);
    });
    dropdown.parent().on('hide.bs.dropdown', function () {
        $(this).append($(this).data("myDropdownMenu"));
        $(this).data("myDropdownMenu").removeAttr('style');
    });

}

$.fn.ToMVCData = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

Array.prototype.remove = function (val) {
    if (typeof (val) != "undefined") {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    }
};
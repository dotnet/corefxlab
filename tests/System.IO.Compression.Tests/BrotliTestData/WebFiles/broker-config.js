/*
Copyright (c) 2016, comScore Inc. All rights reserved.
*/
COMSCORE.SiteRecruit.Broker.config = {
	sv: "scor",
	cddsDomains: 'microsoftstore.com|windowsphone.com|xbox.com|adnxs.com|office.com',
	cddsInProgress: 'cddsinprogress',
	domainSwitch: 'tracking3p',
	domainMatch: '([\\da-z\.-]+\.com)',
	delay: 0,
	cddsIntervalMax: 10,

	crossDomainCheck: function() {
		if (this.cddsIntervalMax > 1) {
			this.cddsIntervalMax --;

			if (COMSCORE.SiteRecruit.Utils.UserPersistence.getCookieValue(this.cddsInProgress) != false ) {
				//COMSCORE.SiteRecruit.DDKeepAlive.setDDTrackerCookie();
				setInterval(function() { COMSCORE.SiteRecruit.DDKeepAlive.setDDTrackerCookie()}, 1000);
				COMSCORE.SiteRecruit._halt = true;
				COMSCORE.SiteRecruit.Utils.UserPersistence.createCookie("srCDDS", "1", {path:'/',domain:COMSCORE.SiteRecruit.Broker.config.cookie.domain,duration:'s'});				
				this.clearCrossDomainCheck();
			}
		}
		else {
			this.clearCrossDomainCheck();
		}
	},

	clearCrossDomainCheck: function() {
		window.clearInterval(crossDomainInterval);
	},

	isolateDomain: function(a) {
		a = a.substring(a.indexOf("//")+2,a.length);
		a = a.substring(0,a.indexOf("/"));
		return a;
	},

	testMode: false,
	addEventDelay: 1000,
	
	cookie:{
		name: 'msresearch',
		path: '/',
		domain:  '.microsoft.com' ,
		duration: 90,
		rapidDuration: 0,
		expireDate: ''
	},
	tracker:{
		std:'http://www.microsoft.com/library/svy/SiteRecruit_Tracker.htm',
		ssl:'https://www.microsoft.com/library/svy/SiteRecruit_Tracker.htm'
	},
	mobile:{
		match: 'iphone|ipad|ipod|android|opera mini|blackberry|windows (phone|ce)|iemobile|htc|nokia|bb10|mobile safari|mobile|wpdesktop|lumia|playbook|tablet',
		kmatch: '(?:; ([^;)]+) Build\/.*)?\bSilk\/([0-9._-]+)\b(.*\bMobile Safari\b)?',
		halt: true
	},
	graceIncr:{
		name: 'graceIncr',
		initDelay: 0,
		clickDelay: 5000,
		match: 'https:\/\/(account|accounts|billing|commerce|support|login|live)\.(microsoft|live|xbox)\.(com)',
		altTag: 'class',
		htmlMatch: 'sign in'
	},
	
	prefixUrl: "",
	
		mapping:[
	// m=regex match, c=page config file (prefixed with configUrl), f=frequency
  {m: '//[\\w\\.-]+/en-(au|ca|eg|hk|in|ie|my|nz|pk|ph|sa|sg|za|gb|us)/learning/.*', c: 'inv_c_Learning-2342.js', f: 0.0, p: 0, halt:true }
  ,{m: '//[\\w\\.-]+/en-us/(cloud-platform$|cloud-platform/.*)', c: 'inv_c_p218292485-3105.js', f: 0.4, p: 0  }
  ,{m: '//[\\w\\.-]+/en-us/dynamics365(?!(crm-(sales|sales2)\\.aspx|contact-us|building-world-class-sales-organization|ray-wang-webinar|how-to-crush-your-sales-quota|how-to-crush-your-sales-quota-(2|3|whitepaper)))', c: 'inv_c_p329946460-14.js', f: 0.5, p: 0 	}
  ,{m: '//[\\w\\.-]+/en-us/evalcenter', c: 'inv_c_p246609455-3128.js', f: 0.5, p: 1  }
	,{m: '//[\\w\\.-]+/en-us/licensing/(?!(servicecenter|licensewise/|mla/))', c: 'inv_c_Licensing-43.js', f: 0.34, p: 0  }
	,{m: '//[\\w\\.-]+/en.*/server-cloud/', c: 'inv_c_p218292485-2197.js', f: 0.0, p: 0, halt:true }
	,{m: '//[\\w\\.-]+/en.*/server-cloud/(sql-|products/sql-server)', c: 'inv_c_p218292485-2198.js', f: 0.4, p: 1  }
	,{m: '//[\\w\\.-]+/en-\\w\\w/sql-server', c: 'inv_c_p218292485-2198.js', f: 0.4, p: 1  }
	,{m: '//[\\w\\.-]+/en-us/trustcenter', c: 'inv_c_p346424086.js', f: 0.5, p: 0  }
	,{m: '(//[\\w\\.-]+/sql/experience/(Default\\.aspx\\?loc=en)|(html/Default\\.aspx\\?loc=en)|(html/Events\\.aspx\\?loc=en)|(LearnSQL\\.aspx\\?h=t&loc=en)|(LearnSQL\\.aspx\\?loc=en)|(Events\\.aspx\\?loc=en)|(.*\\.wmv))|(/learning/en/us/(s|S)yndication(p|P)age\\.aspx)', c: 'inv_c_blank.js', f: 0, p: 3  ,halt: true  }
	,{m: 'powerbi.microsoft.com/en.*', c: 'inv_c_p218292485_2694.js', f: 0.5, p: 1  }
],

	//events
	Events: {
		beforeRecruit: function() {
			// ADD shortcuts
			var csbc = COMSCORE.SiteRecruit.Broker.config;
			var csuu = COMSCORE.SiteRecruit.Utils.UserPersistence;
			
			// IF TRACKING3P EXISTS, REMOVE IT
			if (csuu.getCookieValue(csbc.domainSwitch) != false) {
				csuu.createCookie(csbc.domainSwitch, '', {path:'/',domain:csbc.cookie.domain,duration:-1});
			}
			
			// MS Brand Fav Referrer check for recruitment
			   if(/news\.microsoft\.com\/(stories|features)\/(computingcancer|(data)|further-products-turning-grease-into-soap-as-a-family-business|partnership-between-microsoft-and-nfl-continues-to-score-high-for-players-coaches-and-fans)/i.test(window.location) && !(/site\=/i.test(window.location))) {
			   	var captRef = COMSCORE.SiteRecruit.Utils.UserPersistence.getCookieValue("captRef");
			   	if(/www\.microsoft\.com\/en-us/i.test(captRef)){
			    		    COMSCORE.SiteRecruit._halt = false;
			   		}else{
			    		    COMSCORE.SiteRecruit._halt = true;}
			   	}
			   	if(/www\.microsoft\.com\/tate/i.test(window.location)) {
			    	if((/int\_cle/i.test(window.location))){
			    	 if((/www\.microsoft\.com\/en-us/i.test(window.location))){ 
			    	 	    COMSCORE.SiteRecruit._halt = false;
			    	 }else{			    	 	    
			    		    COMSCORE.SiteRecruit._halt = true;}
			    	}else if(!(/www\.microsoft\.com\/en-us/i.test(document.referrer))|| (/int\_cle/i.test(document.referrer))){
			    		 COMSCORE.SiteRecruit._halt = true;			    		 
			    	}
			   }

			if (/windows.com/i.test(document.domain)) {
				COMSCORE.SiteRecruit.Broker.config.cookie.domain = '.windows.com';
			}

			//CDDS p246609455 - Referrer Check
			if(/(en-us\/default\.aspx|en-us?$|en-us\/?$)|(en-us\/download)|(download\/(en\/|.*?displaylang=en))/i.test(window.location.toString()) &&/windowsmediaplayer|(microsoft|microsoftbusinesshub|xbox|windowsphone|windows|skype|live|microsoftvirtualacademy|microsoftvolumelicensing|microsoftstore|office|office365|onenote|visualstudio|outlook|technet|msdn)\.com/i.test(document.referrer) ) {       
           COMSCORE.SiteRecruit._halt = true; 
      }
      		
			//Mobile
			if (/(en-us\/default\.aspx|en-us?$|en-us\/?$)/i.test(window.location)) {
        if(COMSCORE.SiteRecruit.device.type>1){
          csuu.createCookie("srMOB", "1", {path:'/',domain:csbc.cookie.domain,duration:'s'})
          COMSCORE.SiteRecruit.Broker.config.mobile.halt = false;
        }
			}
			
			COMSCORE.SiteRecruit.Broker.custom = {
				captLinks: function(u) {
					var v = csuu.getCookieValue('captLinks');
					var c = "";

					if (v == false) {
						c = escape(u) + ';';
					}
					else {
						if (c.length + v.length < 1440) {
							c = v + escape(u) + ';';
						}
					}

					if (c != "") {
						csuu.createCookie('captLinks', c, {path:'/',domain:csbc.cookie.domain,duration:'s'});
					}
				},

				allTags: function(x,x1,y,z) {
					/*
						x:  Tag type
						x1: Alt Match pattern
						y:  Match pattern
						z: 
		  	  				1 - CDDS
		  	  				2 - graceIncr
		  	  				3 - captLinks
					*/

					if (x == 'class') {
						if (/msie (8|7)/i.test(navigator.userAgent)) { return; }
						var aTags = document.getElementsByClassName(x1);
					}
					else {
						var aTags = document.getElementsByTagName(x);
					}

					var sr_r = new RegExp(y,'i');
					for (var i = 0; i < aTags.length; i++) {
				 		if ( (x == 'a' && sr_r.test(aTags[i].href)) || (x == 'class' && sr_r.test(aTags[i].innerHTML))	) {
							if (aTags[i].addEventListener) {
								this.href = aTags[i].href;
								if (z == 1) {
									aTags[i].addEventListener('click', function(event) {
										if (/go.microsoft.com/i.test(this.href) ) {
											var _clickURL = "";
											if (/LinkId\=258855/i.test(this.href)) {	_clickURL = "http://www.microsoftstore.com/store";	}
											if (/LinkId\=200036/i.test(this.href)) {	_clickURL = "http://www.xbox.com";	}
											if (/LinkId\=198372/i.test(this.href)) {	_clickURL = "http://www.windowsphone.com";	}
											if (/LinkId\=258852/i.test(this.href)) {	_clickURL = "http://products.office.com";	}

											if (_clickURL != "") {
												csuu.createCookie(csbc.domainSwitch, _clickURL, {path:'/',domain:csbc.cookie.domain,duration:'s'});
											}
										}
										else {
											if (sr_r.test(this.href)) {	csuu.createCookie(csbc.domainSwitch, this.href, {path:'/',domain:csbc.cookie.domain,duration:'s'})	}	
										}
									}, false);
								}
								else if (z == 2) {
									aTags[i].addEventListener('click',function(event){	csuu.createCookie("graceIncr", 1, {path:'/',domain:csbc.cookie.domain,duration:'s'})	},false);
								}
								else if (z == 3 && COMSCORE.isDDInProgress()) {
									aTags[i].addEventListener('click',function(event){ COMSCORE.SiteRecruit.Broker.custom.captLinks(this.href)	},false);	
								}
							}
							else if (aTags[i].attachEvent) {
								if (z == 1) {
									aTags[i].attachEvent('onclick', function(e) {
										if (/go.microsoft.com/i.test(this.href) ) {
											var _clickURL = "";
											if (/LinkId\=258855/i.test(this.href)) {	_clickURL = "http://www.microsoftstore.com/store";	}
											if (/LinkId\=200036/i.test(this.href)) {	_clickURL = "http://www.xbox.com";	}
											if (/LinkId\=198372/i.test(this.href)) {	_clickURL = "http://www.windowsphone.com";	}
											if (/LinkId\=258852/i.test(this.href)) {	_clickURL = "http://products.office.com";	}

											if (_clickURL != "") {
												csuu.createCookie(csbc.domainSwitch, _clickURL, {path:'/',domain:csbc.cookie.domain,duration:'s'});
											}
										}
										else {
											if (sr_r.test(e.srcElement)) {	csuu.createCookie(csbc.domainSwitch, e.srcElement, {path:'/',domain:csbc.cookie.domain,duration:'s'})		}
										}
									});
								}
								else if (z == 2) {
									aTags[i].attachEvent('onclick',function()	{	csuu.createCookie("graceIncr", 1, {path:'/',domain:csbc.cookie.domain,duration:'s'})	});
								}
								else if (z == 3 && COMSCORE.isDDInProgress()) {
									aTags[i].attachEvent('onclick',function()	{ 	COMSCORE.SiteRecruit.Broker.custom.captLinks(e.srcElement)	} );	
								}
							}
							else {}
						}
 					}
				},
				
				checkClickTaleData: function() {
          if(intMax > 0){
            intMax --;			
            try{
              if(ClickTaleGetPID() && ClickTaleGetSID() && ClickTaleGetUID()){
                COMSCORE.SiteRecruit.clickTaleData = ClickTaleGetPID() + "," + ClickTaleGetSID() + "," + ClickTaleGetUID();
                var c = 'sr_CT=' + COMSCORE.SiteRecruit.clickTaleData + '; path=/' + '; domain=' + COMSCORE.SiteRecruit.Broker.config.cookie.domain;
                document.cookie = c; 
                //("CT data loaded: " + COMSCORE.SiteRecruit.clickTaleData);
                COMSCORE.SiteRecruit.Broker.custom.clearClickTaleData();
              }
            }catch(err){	}
          }else{
            COMSCORE.SiteRecruit.Broker.custom.clearClickTaleData();
          }
        },
      
        clearClickTaleData: function() {
          window.clearInterval(CTDInterval);
        },
      checkCaptRef: function() { 
	      if(intMax > 0){     
		    intMax --;	
		    try{	  
          var allLinks = document.getElementsByTagName("a");
          for (var i = 0, n = allLinks.length; i < n; i++){
	         if (/www\.microsoft\.com\/en-us/i.test(window.location)) {
	          //if(/news\.microsoft\.com\/(stories|features|)\/(data|partnership-between-microsoft-and-nfl-continues-to-score-high-for-players-coaches-and-fans)/i.test(allLinks[i])){
	           if(/news\.microsoft\.com\/((\?post_type\=features\&p\=319310)|((stories|features|)\/(data|partnership-between-microsoft-and-nfl-continues-to-score-high-for-players-coaches-and-fans)))/i.test(allLinks[i])){
	            var _captRef = COMSCORE.SiteRecruit.Utils.UserPersistence.getCookieValue("captRef");
              if(!_captRef ){
	              var c = 'captRef=' + document.location + ';path=/' + '; domain=.microsoft.com';
                document.cookie = c;
            }}}}
        }catch(err){	}
        }else{
         COMSCORE.SiteRecruit.Broker.custom.clearCaptRef();
        }
      },

      clearCaptRef: function() {
          window.clearInterval(CaptRefInterval);
        }

			};

      // Initialize CT data
      COMSCORE.SiteRecruit.clickTaleData = '';
      if(/en-us\/windows\/10/i.test(document.location.toString()) && document.cookie.indexOf('sr_CT') == -1 ){
        var intMax = 15;
        var CTDInterval = window.setInterval('COMSCORE.SiteRecruit.Broker.custom.checkClickTaleData()', '1000');
      }
      
      if (/www\.microsoft\.com\/en-us/i.test(window.location)) { 
      	var intMax = 15;
        var CaptRefInterval = window.setInterval('COMSCORE.SiteRecruit.Broker.custom.checkCaptRef()', '1000');
      }

			// Initialize graceIncr cookie
			var gIdelay = 0;
			if (COMSCORE.SiteRecruit.Utils.UserPersistence.getCookieValue("graceIncr") == 1) {	gIdelay = 5000;	}
			setTimeout(function(){COMSCORE.SiteRecruit.Utils.UserPersistence.createCookie("graceIncr", 0, {path:'/',domain:csbc.cookie.domain,duration:'s'})},gIdelay);
      setTimeout(function() { COMSCORE.SiteRecruit.Broker.custom.allTags(csbc.graceIncr.altTag,"msame_Header_name msame_TxtTrunc",csbc.graceIncr.htmlMatch,2) }, csbc.addEventDelay );

			// ADD onclick EVENTS FOR CDDS
			setTimeout(function() { COMSCORE.SiteRecruit.Broker.custom.allTags('a','',csbc.cddsDomains,1) }, csbc.addEventDelay );
			setTimeout(function() { COMSCORE.SiteRecruit.Broker.custom.allTags('a','',csbc.graceIncr.match,2) }, csbc.addEventDelay );

					}
	}
};

//CUSTOM - CHECK FOR THE CROSS-DOMAIN COOKIE. IF PRESENT, HALT RECRUITMENT AND SET DD TRACKING COOKIE
var crossDomainInterval = window.setInterval('COMSCORE.SiteRecruit.Broker.config.crossDomainCheck()', '1000');
//END CROSS_DOMAIN DEPARTURE FUNCTIONALITY

//CUSTOM - ADD 5 SECOND DELAY ON CALLING BROKER.RUN()
if (COMSCORE.SiteRecruit.Broker.delayConfig == true)  {
	COMSCORE.SiteRecruit.Broker.config.delay = 5000;
}
window.setTimeout('COMSCORE.SiteRecruit.Broker.run()', COMSCORE.SiteRecruit.Broker.config.delay);
//END CUSTOM
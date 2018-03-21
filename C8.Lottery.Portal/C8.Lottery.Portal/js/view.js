var pa={
	transform:{
		x:0,
		y:0,
		z:0,
		scale:1
		},
	eventType : {
		start : 'touchstart',
		move : 'touchmove',
		end : 'touchend'
	},
	move:false,
	init:function(){
		var _this=this;
		this.setw();
		this.slide();
		$(window).resize(function(){
			_this.setw();
		});
		
		$('.nav-box .line,.all-nav .line').on('click',function(){
			var index=$(this).index();
			$('.all-nav').hide();
			_this.next(index);
		});
		
		
		$('.upbtn').on('click',function(){
			if($(this).is('.on')){
				$(this).removeClass('on');
				$('.all-nav').hide();
			}else{
				$(this).addClass('on');
				$('.all-nav').show();
			}
		});
		
		
		setInterval(function(){
			$('.notice_list').animate({'marginTop':'-38px'},function(){
				$(this).append($(this).find('li').eq(0).clone());
				$(this).find('li').eq(0).remove();
				$(this).css({'marginTop':'0px'});
			});
		},2000)
		
	},
	setw:function(){
		var _this=this;
		var nw=$('.nav-box .line')[0].offsetWidth+25;
		var length=$('.nav-box .line').length;
		var navw=nw*length;
		$('.nav-box').width(navw);
		var w=$(window).width();
		var sw=w*length;
		$('.slick-track').width(sw);
		$('.slick-slide').width(w);
		this.w=w;
		this.nw=nw;
		this.length=length;
		var index=$('.nav-box .on').index();
		this.next(index);		
	},
	slide:function(){
		var _this=this;
		var id=$('#slick');
		var nav=$('#navbox');
		
		id.on(this.eventType.start,{'fn':_this},this.touchTU);//手指接触事件
		id.on(this.eventType.move,{'fn':_this,'ofn':function(yidong){
			var left=_this.left+yidong.x; 
			$('#slick').css({'transform':'translate3d('+left+'px, 0px, 0px)','transition': 'transform 0s'});		
		}},this.touchMoveTU);//手指移动中事件
		
		id.on(this.eventType.end,{'fn':_this,'ofn':function(){
			var w=_this.w/8;	
			var dong=Math.abs(_this.transform.x);
			var index=$('.nav-box .on').index();
			if(dong>w){
				if(_this.transform.x<0&&index<_this.length-1){
					index=index+1;	
				}else if(_this.transform.x>0&&index>0){
					index=index-1;	
				}
			}
			$('.slick-track').css({'transition': 'transform 500ms'});				
			_this.next(index);
			setTimeout(function(){
				var yi=Math.abs(_this.transform.x)||0;
				if(yi<=10&&pa.isMobile()){
					_this.bigger(index);
				}
				_this.transform.x=0;
			},0);
			
		}},this.touchEndTU);//手指离开事件	
		
		nav.on(this.eventType.start,{'fn':_this},this.touchTU);
		nav.on(this.eventType.move,{'fn':_this,'ofn':function(yidong){
			var left=_this.navleft+yidong.x;
			_this.navlef=left;
			$('.nav-box').css({'transform':'translate3d('+left+'px, 0px, 0px)','transition': 'transform 0s'});
			
		}},this.touchMoveTU);
		
		nav.on(this.eventType.end,{'fn':_this,'ofn':function(){
			var min=_this.nw*(_this.length-2)*-1;
			if(_this.navlef>=96){
				_this.navlef=96
			}else if(_this.navlef<=min){
				_this.navlef=min;
			}
			var index=Math.ceil(_this.navlef/_this.nw)*-1+1;
			_this.next(index);
			
		}},this.touchEndTU);
		
		
		$('.paper-warp').on('click',function(){
			$('.pswp__bg_f').hide();
			$('.paper-warp').hide();
			var index=$('.paper-warp').attr('index');
			_this.scale=1;
			_this.next(index);
		})
		$('#opic').on('click',function(e){
			e.stopPropagation();
		})
		
		$('#opic').on(this.eventType.start,{'fn':_this},this.touchTU);
		$('#opic').on(this.eventType.move,{'fn':_this,'ofn':function(yidong){
			if($('#paper-warp').is('.on')){
				var top=yidong.y;
				var left=yidong.x;
				_this.pleft=left;
				$('#paper-warp').css({'transform':'translate3d('+left+'px,'+top+'px, 0px)','transition': 'transform 0s'});		
			}
		}},this.touchMoveTU);
		
		$('#opic').on(this.eventType.end,{'fn':_this,'ofn':function(){
			if(_this.transform.x==0){
				if($('.paper-warp').is('.on')){
					$('.paper-warp').removeClass('on');
					_this.scale=1
				}else{
					$('.paper-warp').addClass('on');
					_this.scale=1.5;
				}
				$('.odiv').css({'transform':'translate3d(0px, 0px, 0px) scale('+_this.scale+','+_this.scale+')','transition': 'transform 500ms'});
				$('#paper-warp').css({'transform':'translate3d(0px,0px,0px)','transition': 'transform 0s'});		
			}else{
				var  left=_this.w*(_this.scale-1)/2;		
				setTimeout(function(){ 
					if(_this.pleft>left){
						_this.pleft=left;
					}
					
					if(_this.pleft<left*-1){
						_this.pleft=left*-1;
					}					
					$('#paper-warp').css({'transform':'translate3d('+_this.pleft+'px,0px,0px)','transition':'transform 500ms'});			
				},0);
				
				
			}
			_this.transform.x=0;
			_this.transform.y=0;	
		}},this.touchEndTU);
	},
	pnext:function(index){
		var _this=this;
		var length=this.length-1;
		if(index<0){
			index=length;
		}
		if(index>length){
			index=0;
		}
		setTimeout(function(){
			$('.paper-warp').css({'transition': 'transform 0ms'});		
			_this.bigger(index);
		},500)
	},
	bigger:function(index){
		var _this=this;
		$('.odiv').width(this.w);
		$('.paper-warp').show();
		$('.pswp__bg_f').show();
		
		this.pleft=0;
		$('.paper-warp').css({'transform':'translate3d(0px, 0px, 0px)','transition':'transform 0ms'});
		$('.odiv').css({'transform':'translate3d(0px, 0px, 0px) scale('+_this.scale+','+_this.scale+')'});
		this.oindex(index);
	},
	oindex:function(index){
		var length=this.length-1;
		var b=index==0?length:index-1;
		var a=index==length?0:index+1;
		
		var src=$('.slick-slide').eq(index).find('img').data('src');
		var qi=$('.nav-box .line').eq(index).html();
		
		$('.pswp_title').html(qi);
		
		$('.center img').attr('src',src);
		$('.paper-warp').attr('index',index);
	},
	touchTU:function(evt){
		var _this=evt.data.fn;
		evt.stopPropagation();
		_this.move=true;
		if(pa.isMobile()){
			var touch = evt.originalEvent.touches[0];
			_this.x = Number(touch.pageX);
			_this.y = Number(touch.pageY);
		}else{
			_this.x = evt.offsetX;
			_this.y = evt.offsetY;
		}
	},
	touchMoveTU:function(evt){
		_this=evt.data.fn;
		evt.preventDefault();
		if(!_this.move){
			return false;
		}
		if(pa.isMobile()){
			var touch = evt.originalEvent.touches[0];
			var x = Number(touch.pageX);
			var y = Number(touch.pageY);
		}else{
			var x  = evt.offsetX;
			var y  = evt.offsetY;
		}
		_this.transform.x=x-_this.x;
		_this.transform.y=y-_this.y;
		evt.data.ofn(_this.transform);
	},
	touchEndTU:function(evt){
		_this=evt.data.fn;
		_this.move=false;
		evt.data.ofn();
	},
	next:function(index){
		var left=this.w*index*-1;
		var navleft=this.nw*(index-1)*-1;
		$('.slick-track').css({'transform': 'translate3d('+left+'px, 0px, 0px)'});
		$('.nav-box').css({'transform': 'translate3d('+navleft+'px, 0px, 0px)'});
		$('.nav-box .line').removeClass('on');
		$('.nav-box .line').eq(index).addClass('on');
		var oimg=$('.slick-slide').eq(index).find('img');
		var src=oimg.data('src');
		oimg.attr('src',src);
		this.index=index;
		this.left=left;
		this.navleft=navleft;
	},
	isMobile : function(){
		var sUserAgent= navigator.userAgent.toLowerCase(),
		bIsIpad= sUserAgent.match(/ipad/i) == "ipad",
		bIsIphoneOs= sUserAgent.match(/iphone os/i) == "iphone os",
		bIsMidp= sUserAgent.match(/midp/i) == "midp",
		bIsUc7= sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4",
		bIsUc= sUserAgent.match(/ucweb/i) == "ucweb",
		bIsAndroid= sUserAgent.match(/android/i) == "android",
		bIsCE= sUserAgent.match(/windows ce/i) == "windows ce",
		bIsWM= sUserAgent.match(/windows mobile/i) == "windows mobile",
		bIsWebview = sUserAgent.match(/webview/i) == "webview";
		return (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM);
     }
}

$(function(){
	if(!pa.isMobile()){
		pa.eventType.start = 'mousedown';
		pa.eventType.move = 'mousemove';
		pa.eventType.end = 'mouseup';
	}
	pa.init();	
});
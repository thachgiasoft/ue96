// JavaScript Document    
$(document).ready(function(){	
    $('#orderBy li:eq(1)').css('padding-right',17);
	$('#orderBy li:eq(1)').css('background-image','url(../static/images/listBg.gif)');
	$('#orderBy li:eq(1)').css('background-repeat','no-repeat');
	$('#orderBy li:eq(1)').css('background-position','39px 3px');		
	//关闭弹出窗
	$('.close').click(function(){
		$(this).parents('.popbox').hide();
	});
	heightQuality();
	//筛选属性(productslist.html,search.html)
	screeningAttr();
	showPhoto();	
	buycart();	
	progressBar();
	introMenu();
	//评价区
	var commentAll=$('#comments .allComment .comment');
	$('#comments .menu li').click(function(){
		selectBox($(this),commentAll);
	});	
	$('#buyArea .buyApp label').click(function(){
		$(this).addClass('selected').siblings().removeClass('selected');
	});		
	//评论撰写窗口
    editComment();
	//评论回复
	$('#comments .allContent .bt6').click(function(){
		var replyBox=$(this).parent('p').next('.replyBox');
		var _display=replyBox.css('display');
		var _htmlText='<p><strong>发表回复：</strong></p><p><textarea></textarea></p><p>请填写回复内容，字数在5到300个字符之间。</p><p><button>提交</button></p>';		
		if(_display=='none')
		{
			replyBox.html(_htmlText);
			replyBox.show();
		}else
		{
			replyBox.hide();
		}
	});
	$('#comments #reply button').click(function(){
		$(this).parents('#reply').hide();
	});
	//提问
	$('#introduce .qa .alignLine .link1').click(function(){
		popShow($('#questions'));
	});
	//回答
	$('#introduce .qa .singleQA .bt3').click(function(){
		popShow($('#answer'));
	});	
	joinPage();
	scrollItem($('#panic'),3,750);
	indexProductShow();
	helpJs();
	$('#ProductIframe').load(function(e) {		
		  setIframe('out');
    });
});
//设置iframe高度
var setIframe=function(position){
	var list,$iFrame;
	switch(position){
		case 'in':
		    list=$('#productList');
			$iFrame=$('#ProductIframe',parent.document);
			break;
		case 'out':
		    list=$('#ProductIframe').contents().find('#productList');
			$iFrame=$('#ProductIframe');
			break;
	}
	var heightSet=function(){
		var _height=list.height();
		if(_height>0){
			$iFrame.height(_height+100);
			clearInterval(interval);
		}
	}
	var interval=setInterval(heightSet,1000);
}
//商品详情菜单(product.html)
var introMenu=function()
{
    var introM = $('.introMenu');
    var introduce = $('#introduce .group');
    $('.introMenu li').click(function () {
        selectBox($(this), introduce);
        introM.css('top', 0);
    });  
    if (introM.attr('class') == 'introMenu') {
        var _top = introM.position().top;
    }
    $(window).scroll(function () {
        if ($(document).scrollTop() > _top) {
            introM.css('top', $(document).scrollTop() - _top);
        } else
		{
			introM.css('top', 0);
        }
    });
}
//精品展示区脚本(index.html)
var heightQuality=function()
{
    var jplen = $('#highgoods1 .item li img').length;
    for (var i = 0; i < jplen; i++) {
        var _img = $('#highgoods1 .item li img').eq(i);
        _img.css('margin-top', (385 - Number(_img.height())) / 2);
    }
    $('#highgoods1 .item li').hover(function () {
        var _imgname = $(this).find('img').attr('src');
        _imgname = _imgname.replace('a.png', 'b.png');
        $(this).find('img').attr('src', _imgname);
        $(this).animate({ 'top': -16 }, 'normal');
    },
	    function () {
	        var _imgname = $(this).find('img').attr('src');
	        _imgname = _imgname.replace('b.png', 'a.png');
	        $(this).find('img').attr('src', _imgname);
	        $(this).animate({ 'top': 0 });
	    }
	);
    var _jpitemlen = $('#highgoods1 .group .item').length;
    var hg1group = $('#highgoods1 .group');
    hg1group.width(1200 * _jpitemlen);
    $('#highgoods1 .next').click(function () {
        var _left = hg1group.position().left;
        if (_left == (2 - _jpitemlen) * 1200) $('#highgoods1 .next').hide();
        $('#highgoods1 .prev').show();
        $('#highgoods1 .selectbj a').eq(-_left / 1200 + 1).addClass('selected').siblings().removeClass('selected');
        hg1group.animate({ 'left': _left - 1200 });
    });
    $('#highgoods1 .prev').click(function () {
        var _left = hg1group.position().left;
        if (_left == -1200) $('#highgoods1 .prev').hide();
        $('#highgoods1 .next').show();
        $('#highgoods1 .selectbj a').eq(-_left / 1200 - 1).addClass('selected').siblings().removeClass('selected');
        hg1group.animate({ 'left': _left + 1200 });
    });
    for (var i = 0; i < _jpitemlen; i++)
        $('#highgoods1 .selectbj').append('<a href="javascript:void(0)"></a>');
    var _hg1bjw = $('#highgoods1 .selectbj').width();
    var _left = (1200 - _hg1bjw) / 2;
    $('#highgoods1 .selectbj').css('left', _left);
    $('#highgoods1 .selectbj a').eq(0).addClass('selected');
    $('#highgoods1 .selectbj a').click(function () {
        var _left;
        var _index = $(this).index();
        $(this).addClass('selected').siblings().removeClass('selected');
        hg1group.animate({ 'left': -_index * 1200 }, function () {
            _left = hg1group.position().left;
            if (_left == (1 - _jpitemlen) * 1200) {
                $('#highgoods1 .next').hide();
                $('#highgoods1 .prev').show();
            } else
                if (_left == 0) {
                    $('#highgoods1 .prev').hide();
                    $('#highgoods1 .next').show();
                } else {
                    $('#highgoods1 .prev').show();
                    $('#highgoods1 .next').show();
                }
        });
    });
    var hg2len;
    var hg2item = $('#highgoods2 .item');
    $('#highgoods2 .next').click(function () {
        var _index = $(this).parents('.item').index();
        var scrollw = hg2item.eq(_index).find('.scrollw');
        hg2len = scrollw.children('.scroll').length;
        var _top = scrollw.position().top;
        if (_top == (2 - hg2len) * 158) $(this).hide();
        hg2item.eq(_index).find('.prev').show();
        scrollw.animate({ 'top': _top - 158 });
    });
    $('#highgoods2 .prev').click(function () {
        var _index = $(this).parents('.item').index();
        var scrollw = hg2item.eq(_index).find('.scrollw');
        hg2len = scrollw.children('.scroll').length;
        var _top = scrollw.position().top;
        if (_top == -158) $(this).hide();
        hg2item.eq(_index).find('.next').show();
        scrollw.animate({ 'top': _top + 158 });
    });
}
//首页新品促销
var indexProductShow=function(){
	var $menu=$('.Promotions dt a');
	var $content=$('.Promotions dd ul');
	$menu.eq(0).addClass('selected');
	$content.eq(0).addClass('selected');
	$menu.mouseover(function(e) {
        selectBox($(this),$content);
    });
}
//选择框函数	
var selectBox=function(menu,content){
	var _index=menu.index();
	menu.addClass('selected').siblings().removeClass('selected');
	content.eq(_index).addClass('selected').siblings().removeClass('selected');
}
//检测注册信息
var checkRegister=function(obj){
	var _name,_val,_len;
	var warning=obj.parent('p').next('.note').children('strong');
	var mailReg=/^\w+((-\w+)|(\.\w+))*\@{1}\w+\.{1}\w{2,4}(\.{0,1}\w{2}){0,1}/ig;
	_name=obj.attr('name');
	_val=obj.val();
	if(_val==''){
		warning.text(_name+'不能为空').show();
		return;
	}
	switch(_name){
		case'邮箱':
		    if(_val.search(mailReg)==-1){
				warning.text('邮箱格式错误').show();
			}
			break;
		case'用户名':
			if(_val.length<4){
				warning.text('用户名长度为4-20位字符').show();
			}
			break;
		case'密码':
			if(_val.length<6){
				warning.text('密码长度为6-20位字符').show();
			}
			break;
		case'确认密码':
			var _password=$('#login_global #password').val();
			if(_val!=_password){
				warning.text('两次密码输入不一致').show();
			}
			break;
	}
}
//登入注册页(login.html)
var joinPage=function(){
	var login=$('#login_global .login');
	var register=$('#login_global .register');
	var input=$('#login_global .input input');
	var defInput=$('#login_global .default');
	var regInput=$('#login_global .register .input input');
	var act=getQueryString('act');
	if(act=='register'){
		login.removeClass('mastW').addClass('slaveW');
		register.removeClass('slaveW').addClass('mastW');
	}
	defInput.click(function(){
		$('#login_global .default').val('');
		$(this).removeClass('default');	
		$(this).css('color','#000');		
	});
	input.focus(function(){
		$(this).addClass('selected');
		$(this).parent('p').next('.note').children('strong').hide();
		$(this).parent('p').next('.note').children('span').show();
	});
	input.blur(function(){
		$(this).removeClass('selected');
		$(this).parent('p').next('.note').children('span').hide();
	});
	regInput.blur(function(){
		checkRegister($(this));
	});	
}
//评价进度条(product.html)
var progressBar=function(){
	var tage,i;
	var url;
	for(i=0; i<3; i++)
	{
		tage=96*Number($('.progressBar span').eq(i).text().match(/[0-9]{1,}/))/100;
		url='url(../images/showbg.png) -'+Math.round(142-tage)+'px -380px no-repeat';
		$('.progressBar').eq(i).css('background',url);
	}
}
//购物车确认窗口
var buycart=function(){
	var cartcheck=$('#cartCheck');	
	var goodsInfo=$('#cartCheckDetail');
	scrollItem(cartcheck,3,408);
	$('.bt7').click(function(){
		popShow(cartcheck);
		getInfo();			
	});	
	$('.bt4').click(function(){
		popShow(cartcheck);	
		getInfo();
	});
	var getInfo=function(){
		var _src=$('.photoItem li:eq(0) img').attr('src');
		var _name=$('#productBriefName').val();
		var _num=$('#buyArea .buyApp .num').val();
		var _price=$('#buyArea .buyInfo .yyj').text();
		$('#cartCheckImg img').attr('src',_src);
		goodsInfo.children('h2').text(_name);
		goodsInfo.children('p').first().children('span').text(_num);
		goodsInfo.children('p').last().children('span').text('¥'+_price*_num);
	}
	
}
//弹出窗口定位显示函数
var popShow=function(obj)
{
	var _winHeight=$(window).height();
	var _height=(_winHeight-obj.outerHeight())/2+$(document).scrollTop();	
	obj.css('top',_height);
	obj.show();
}
//商品详情照片显示
var showPhoto=function(){
	var magnifier=$('.magnifier');
	var bigWindow=$('.bigWindow');	
	var sP=$('.smallPhoto ul li');
	var bigImg=$('.bigPhoto img');
	var x,y,top,left,bigWidth,scale;
	var doSome=function(obj)
	{
		obj.addClass('sel').siblings().removeClass('sel');
		$('.bigPhoto').html(obj.html());
		bigWindow.css('background','url('+$('.bigPhoto img').attr('src')+') '+' no-repeat #fff');	
		bigWidth=getImg($('.bigPhoto img'));
		scale=(bigWidth-400)/220;
	}
	scrollItem($('#photoShow'),5,330);
	doSome(sP.eq(0));
	sP.mouseover(function(){
		doSome($(this));
	});		
	$('.bigPhoto').mousemove(function(event){		
	    bigWindow.position().top=$('.bigPhoto').position().top;    
		x=event.pageX-$(this).offset().left;
		y=event.pageY-$(this).offset().top;
		magnifier.show();
		magnifier.css('top',y-97);
		magnifier.css('left',x-97);	
	});	
	magnifier.mousemove(function(event){
		var ox=$(this).position().left+(event.pageX-$(this).offset().left-77);
		var oy=$(this).position().top+(event.pageY-$(this).offset().top-77);
		if(ox<0) ox=0
		else if(ox>220) ox=220;
		if(oy<0) oy=0
		else if(oy>220) oy=220;		
		$(this).css('top',oy+'px');
		$(this).css('left',ox+'px');		
		bigWindow.show().css('background-position',(0-ox*scale)+'px '+(0-oy*scale)+'px');
	});	
	magnifier.mouseout(function(){
		$(this).hide();
		bigWindow.hide();
	});	
}
//获取图片真实高宽
var getImg=function(img)
{
	var imgTemp=new Image();
	var heightTemp,widthTemp;
	var src=img.attr('src');
	imgTemp.src=src;
	widthTemp=imgTemp.width;
	imgTemp=null;
	return widthTemp;
}
//评论撰写窗口
var editComment=function(){
	var commentBt=$('#comments .comments-head .right .bt1');
	var level=$('#fComment .level a');
	commentBt.click(function(){
		popShow($('#fComment'));
	});
	level.click(function(){
		var _index=$(this).index();
		if($(this).attr('class')=='selected')
		{
			if(_index>0 & level.eq(_index+1).attr('class')!='selected')
			{
				$(this).removeClass('selected');
			}			
		}else
		{
			if(_index>0 & level.eq(_index-1).attr('class')=='selected')
			{
				$(this).addClass('selected');
			}
		}
	});
}
//筛选属性(productslist.html)
var screeningAttr=function(){
  var singleAttr=$('#screening .attr a');
  var moreAttr=$('#screening .more a');
  var itemAttr=$('#screening .attr');
  if(itemAttr.length>4)
  {
	  $('#screening .attr:gt(3)').hide();
  }else
  {
	  $('#screening .more *').hide();
  }
  singleAttr.click(function(){
	  $(this).parents('.attr').find('a').removeClass('selected');
	  $(this).addClass('selected');
  });
  moreAttr.click(function(){
	  if($(this).text()=='更多筛选')
	  {
		  $('#screening .attr:gt(3)').show();
		  $(this).text('收起');
	  }else
	  {
		  $('#screening .attr:gt(3)').hide();
		  $(this).text('更多筛选');
	  }
  });
}
//横向滚动组件
var scrollItem = function (obj, cLen, cWidth) {
    var prev = obj.find('.scrollItem .prev a');
    var next = obj.find('.scrollItem .next a');
    var showItem = obj.find('.scrollItem ul');
    var column = obj.find('.scrollItem ul li');
    var dot = obj.find('.dot');
    var len = column.length;
    var itemNum = Math.ceil(len / cLen);
    var width = column.outerWidth();
    var itemWidth = itemNum * cWidth;
    showItem.css('width', itemWidth);
    for (var i = 0; i < itemNum; i++) {
        dot.append("<a href='javascript:'></a>");
    }
    dot.children('a').eq(0).addClass('selected');
    if (len > cLen) {
        next.show();
    }
    var cDot = dot.children('a');
    cDot.click(function () {
        $(this).addClass('selected').siblings().removeClass('selected');
        var _index = $(this).index();
        var _left = -_index * cWidth;
        if (_index == 0) { prev.hide(); next.show(); }
        if (_index == itemNum - 1) { next.hide(); prev.show(); }
        showItem.animate({ 'left': _left });
    });
    next.click(function () {
        var _index = dot.children('.selected').index();
        var _left = showItem.position().left;
        if (_left == cWidth * 2 - itemWidth) {
            $(this).hide();
        }
        prev.show();
        cDot.eq(_index + 1).addClass('selected').siblings().removeClass('selected');
        showItem.animate({ 'left': _left - cWidth });
    });
    prev.click(function () {
        var _index = dot.children('.selected').index();
        var _left = showItem.position().left;
        if (_left == -cWidth) {
            $(this).hide();
        }
        next.show();
        cDot.eq(_index - 1).addClass('selected').siblings().removeClass('selected');
        showItem.animate({ 'left': _left + cWidth });
    });
}
//帮助中心
var helpJs=function(){
	var pName=window.location.pathname;
	var helpM=$('.helpMenu dt');
	var currentPage=$('.helpMenu dd a').filter(function(index) {
		var hre=$(this).attr('href');
		if(pName.search(hre)!==-1){
			return $(this);
		}
    });
	var menuTitle=currentPage.parents('dd').prevAll('dt');
	currentPage.addClass('sel');
	menuTitle.addClass('selected');
	menuTitle.nextAll('dd').show();
	helpM.click(function(){
		var _class=$(this).attr('class');
		if(_class=='selected'){
			$(this).removeClass('selected');
			$(this).nextAll('dd').hide();
		}
		else{
			$(this).addClass('selected');
			$(this).nextAll('dd').show();
			$(this).parent('dl').siblings().children('.selected').removeClass('selected');
			$(this).parent('dl').siblings().children('dd').hide();
		}
	});	
}
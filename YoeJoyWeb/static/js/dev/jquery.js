$(document).ready(function() {		
	scrollLoad();
    focues();
	notes();	
	chart();
	sorts();
	seachBox();
	pro();
	auLabel();
	processChart();
	MyYoyi();
	addNewAdress();	
	Meun2();	
	onlineComplain();
	ToLook();	
	var box=$('#newFocus');		
	History(box,3,552);
	classNav();
	adItemShow();
});
$(window).load(function(e) {
	$('#bg').addClass('allDom');
});
//屏载组件
function scrollLoad(){	
	var _top,j=0;	
	var im=$('.imgshow');
	var _len=im.length;
	var topArr=new Array;		
	var scrollShow=function(){
		if(j==0)
		{
			for(var i=0; i<_len; i++)
				  {
					  topArr[i]=Math.floor(im.eq(i).position().top);
				  }	
		}	
		_top=$(window).height()+$(document).scrollTop();
		if(_top>topArr[0])
		{
			im.eq(j).find('img').attr('src',function(){
				return $(this).attr('data-src');
			});
			topArr.splice(0,1);	
			j+=1;
		}	
	}
	var ss=setInterval(scrollShow,300);
	setTimeout(function(){clearInterval(ss);},10000);
	$(window).scroll(function(){
		ss=setInterval(scrollShow,300);
		setTimeout(function(){clearInterval(ss);},10000);
	});
}
//大类页主广告滚动脚本
function adItemShow(){
	var bt=$('#adShow .btItem a');
	$('#adShow a:eq(0)').show();
	bt.eq(0).addClass('selected');
	var adSetRoll=setInterval(adRoll,3000);
	bt.click(function(){
		var _index=$(this).index();
		$(this).addClass('selected').siblings().removeClass('selected');
		$('#adShow a').eq(_index).addClass('show').siblings('#adShow a').removeClass('show');
	});
	$('#adShow').hover(function(){clearInterval(adSetRoll);},function(){adSetRoll=setInterval(adRoll,3000);});
	function adRoll(){
		var _index=$('#adShow .show').index();
		if(_index<2){
			$('#adShow a').eq(_index+1).addClass('show').siblings('#adShow a').removeClass('show');
			bt.eq(_index+1).addClass('selected').siblings().removeClass('selected');
		}
		else{
			$('#adShow a').eq(0).addClass('show').siblings('#adShow a').removeClass('show');
			bt.eq(0).addClass('selected').siblings().removeClass('selected');
		}
	}
}
//获取url参数函数
var getQueryString=function(name){
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
	var r = window.location.search.substr(1).match(reg);
	if (r != null) return unescape(r[2]); return null;
}
//内页导航伸缩组件
function classNav() {
    var className = $('.listOut h3');
	var classID,cName;
	var fName=window.location.pathname;
	if(fName.search('SubProductList1')!==-1)
	{
		$('.listOut li').addClass('sel');
	}else
	{
		if(fName.search('SubProductList2')!==-1)
		{
			classID=getQueryString('c2');
		}else
		if(fName.search('SubProductList3')!==-1)
		{
			classID=getQueryString('c3');
		}
		cName=$('.listOut input').filter(function(index) {
			return $(this).val()==classID;
		});
		cName.parents('li').addClass('sel');	
	}
    className.click(function () {
		var p=$(this).nextAll('p');
        var _display = p.css('display');
		var li=$(this).parent('li');
        if (_display == 'block') {
            li.removeClass('sel');
        } else {
            li.addClass('sel');
        }
    });
}

function ToLook(){
	var ToLook=$('.ToLook');
	ToLook.hover(function(){
		$(this).css('background','#FFF2DB');	
	},function(){
		ToLook.css('background','#FFF');				
	});	
}

function History(element,cLen,cWidth){
	var prev=element.find('.prev a');
	var next=element.find('.next a');
	
	var showItem=$('#animate');
	var column=$('#animate span');
	var dot=$('#NumTitle');
	
	var len=column.length;	
	var itemNum=Math.ceil(len/cLen);
	
	var width=column.outerWidth();
	var itemWidth=itemNum*cWidth;	
	showItem.css('width',itemWidth);
	
	for(var i=0; i<itemNum; i++)
	{
		dot.append("<a href='javascript:'></a>");
	}
	dot.children('a').eq(0).addClass('sel');
	if(len>(cLen-1)){
		next.show();
	}	
	var cDot=dot.children('a');
	
	cDot.click(function(){
		$(this).addClass('sel').siblings().removeClass('sel');
		var _index=$(this).index();
		var _left=-_index*cWidth;
		if(_index==0){ prev.hide(); next.show();}
		if(_index==itemNum-1){ next.hide(); prev.show();}
		showItem.animate({'left':_left});
	});
	
	next.click(function(){
		var _index=dot.children('.sel').index();
		var _left=showItem.position().left;	
		if(_left==cWidth*2-itemWidth){
			$(this).hide();
		}			
		prev.show();
		cDot.eq(_index+1).addClass('sel').siblings().removeClass('sel');
		showItem.animate({'left':_left-cWidth});
		return false;
	});
	
	prev.click(function(){
		var _index=dot.children('.sel').index();
		var _left=showItem.position().left;			
		if(_left==-cWidth){
			$(this).hide();
		}	
		next.show();	
		cDot.eq(_index-1).addClass('sel').siblings().removeClass('sel');	
		showItem.animate({'left':_left+cWidth});
		return false;
	});	

}




function onlineComplain(){
	var th=$('#onLineComplain th');
	var one=$('.one1');
	var two=$('.two1');
	var address=$('.addNewAddress');
	var NewAddressContent=$('.NewAddressContent');

	
	address.click(function(){
		NewAddressContent.css('display','block');	
	});
	
	th.click(function(){
		var i=th.index(this);
		th.removeClass('sel');
		$(this).addClass('sel');	
		if(i==0){
			one.show();
			two.hide();			
		}
		else if(i==1){
			two.show();
			one.hide();							
		}

	});
}

function addNewAdress(){
	var model=$('.model');
	var save=$('.save');
	var save1=$('.save1');
	var modify=$('.modify');
	var choiced=$('.choiced');
	
	var NewAdressContent=$('#NewAdressContent');
	var myNowAddress=$('.myNowAddress');
	var addNewAddr=$('.addNewAdress');
	var invoiceFill=$('#invoiceFill');
	
	addNewAddr.click(function(){
		NewAdressContent.css('display','block');
		
	});
	
	
	modify.eq(0).click(function(){
		NewAdressContent.css('display','block');		
	});
	modify.eq(1).click(function(){
		model.eq(0).css('display','block');		
	});
	
	modify.eq(2).click(function(){
		model.eq(1).css('display','block');		
	});
	
	modify.eq(3).click(function(){
		model.eq(2).css('display','block');		
	});
	modify.eq(4).click(function(){
		model.eq(3).css('display','block');		
	});


	
	save.eq(0).click(function(){
		NewAdressContent.css('display','none');	
		myNowAddress.css('display','block');
		model.eq(0).css('display','block');
		addNewAddr.css('display','none');
	});
	
	save.eq(1).click(function(){
		model.eq(0).css('display','none');
		model.eq(1).css('display','block');
		choiced.eq(0).css('display','block');
	});
	save.eq(2).click(function(){
		model.eq(1).css('display','none');
		model.eq(2).css('display','block')	
		choiced.eq(1).css('display','block');
		
	});
	save.eq(3).click(function(){
		model.eq(2).css('display','none');
		model.eq(3).css('display','block');
		choiced.eq(2).css('display','block');
		
	});
	
	save1.click(function(){
		invoiceFill.css('display','none');	
	});
	
}

function MyYoyi(){
	var MyYoyi=$('#MyYoyi');
	var MyYoyis=$('.MyYoyi');
	MyYoyi.hover(function(){
		MyYoyis.addClass('MyYoyis');
		MyYoyis.find('em').css('display','block');	
	},function(){
		MyYoyis.removeClass('MyYoyis');
		MyYoyis.find('em').css('display','none');						
	});	
}

function processChart(){
	var mycart=$('#Mycart');
	var chartContent=$('#chartContent');
	mycart.hover(function(){
		chartContent.css('display','block');	
	},function(){
		chartContent.css('display','none');	
	});	
}

function auLabel(){
	var num=0;
	var num2=0;
	var timeout=2000;
	var phone1=$('#bigclass1 #phone a');
	var phonecon1=$('#bigclass1 #phoneCon dl');
	
	var phone2=$('#bigclass2 #phone a');
	var phonecon2=$('#bigclass2 #phoneCon dl');
	var set=window.setInterval(au,timeout);
	
	phone1.eq(0).addClass('sel');
	phone2.eq(0).addClass('sel');
	phonecon1.hide();
	phonecon2.hide();
	
	phonecon1.eq(0).show()
	phonecon2.eq(0).show()	
	
	function au(){
		num<phone1.length-1?num++:num=0;
		num2<phone2.length-1?num2++:num2=0;
		phone1.removeClass('sel');
		phone2.removeClass('sel');
		phone1.eq(num).addClass('sel').siblings().removeClass('sel');
		phone2.eq(num2).addClass('sel').siblings().removeClass('sel');
		phonecon1.eq(num).show().siblings().hide();
		phonecon2.eq(num2).show().siblings().hide();
	}
	
	phone1.hover(function(){
		window.clearInterval(set);
		var i=phone1.index(this);
		phone1.removeClass('sel');
		phone1.eq(i).addClass('sel').siblings().removeClass('sel');
		phonecon1.eq(i).show().siblings().hide();
	},function(){
		set=window.setInterval(au,timeout);	
	});
	
	phone2.hover(function(){
		window.clearInterval(set);
		var i=phone2.index(this);
		phone2.removeClass('sel');
		phone2.eq(i).addClass('sel').siblings().removeClass('sel');
		phonecon2.eq(i).show().siblings().hide();
	},function(){
		set=window.setInterval(au,timeout);	
	});
	
	phonecon1.hover(function(){
		window.clearInterval(set);		
	},function(){
		set=window.setInterval(au,timeout);		
	});
	phonecon2.hover(function(){
		window.clearInterval(set);		
	},function(){
		set=window.setInterval(au,timeout);		
	});
}

function pro() {
	var menu1=$('#content .ThreeRow:eq(0) .sortHeader li');
	var content1=$('#content .ThreeRow:eq(0) .main .sort1Con');
	var menu2=$('#content .ThreeRow:eq(1) .sortHeader li');
	var content2=$('#content .ThreeRow:eq(1) .main .sort1Con');
	var menu3=$('#scheme .sortHeader li');
	var content3=$('#scheme .main .sort2Con');	
	menu1.eq(0).addClass('selected');
	content1.eq(0).addClass('selected');	
	menu2.eq(0).addClass('selected');
	content2.eq(0).addClass('selected');
	menu3.eq(0).addClass('selected');
	content3.eq(0).addClass('selected');
	menu1.mousemove(function(){
		selectBox($(this),content1);
	});
	menu2.mousemove(function(){
		selectBox($(this),content2);
	});
	menu3.mousemove(function(){
		selectBox($(this),content3);
	});
}

function seachBox(){
	var s=$('#SearchBox input');
	var p=$('#SearchBox p');
	
	s.focus(function(){
		s.val("");
		s.css('outline','none');
		p.addClass('sel');	
	});
	s.blur(function(){
		p.removeClass('sel');	
	});
}


function sorts(){	
	var sorts=$('#ItemSortCon');
	var sortsContent=$('.classMenu');
	var span=$('#ItemSort span b');
	
	sorts.hover(function(){
		sorts.addClass('ItemSort');
		sortsContent.css('display','block');	
		
		if(span.text()=="+"){
			span.text("-");	
		}
		else if(span.text()=="-"){
			span.text("+");		
		}
		
	},function(){
		sorts.removeClass('ItemSort');		
		sortsContent.css('display','none');	
		if(span.text()=="+"){
			span.text("-");	
		}
		else if(span.text()=="-"){
			span.text("+");		
		}
		
	});
	
}


function chart(){
	var char=$('#count .chartBt');
	var charf=$('#count');
	var charContent=$('#chartContent');
	var car=$('#chart .chartBt');
	
	char.hover(function(){
		charContent.css('display','block');
		car.addClass('sel');
	},function(){
	});
	
	charf.hover(function(){
	},function(){
		charContent.css('display','none');
		car.removeClass('sel');
	});
	
	charContent.hover(function(){
		charContent.show();
		car.addClass('sel');
	},function(){
		charContent.hide();	
		car.removeClass('sel');
	});	
}


function Meun2(){
	var Menu=$('#Menu');
	var focusNum=$('#focus dd a');
	var hovers=$('#Menu .hover');
	var liHover=$('.liHover');
	var mem=$('.Listcontent dt .mem span');	
	var c1,ca,_end;
	var mLen=hovers.length;
	var mHeight=65*mLen+3;
	var cName=Menu.attr('class');
	for(var i=0; i<mLen; i++)
	{
		c1=hovers.eq(i).find('.ListMain h2').text();
		ca=hovers.eq(i).find('.ListMain h2 a').attr('href');
		_end=c1.indexOf('>');
		c1=c1.substring(1,_end);
		c1="<a href='"+ca+"'>"+c1+"频道</a>";
		mem.eq(i).html(c1);
	}
	$('#Menu .hover:gt(4)').hide();
	$('.classMenu .hover').show();
	hovers.eq(0).children('.liHover').css('border-top-width',0);
	hovers.eq(0).children('.liHover').css('padding-top',2);
	Menu.hover(function(){
		focusNum.css('visibility','hidden');
		if(cName!=='classMenu')
		{
		    $('.lbg').animate({height:mHeight},'fast');	
          	$('#Menu .hover').show();
		}
	},function(){
		liHover.removeClass('hov');	
		focusNum.css('visibility','visible');
		if(cName!=='classMenu')
		{
		    $('#Menu .hover:gt(4)').hide();
		    $('.lbg').animate({height:'328'});
		}
	});
	hovers.hover(function(){
		liHover.removeClass('hov');
		$(this).find('.liHover').addClass('hov');
		$(this).find('.Listcontent').show();
	},function(){
			$(this).find('.Listcontent').hide();
	});
}

function focues(){
	var a=$('#focus dt a');
	var n=$('#focus dd a');
	var num=0;
	var timeout=3000;
	var set=window.setInterval(au,timeout);
	
	a.eq(0).show();
	n.css('opacity',0.5);
	n.eq(0).addClass('sel').css('opacity',1);


	n.hover(function(){
		window.clearInterval(set);
		var i=n.index(this);
		n.eq(i).addClass('sel').siblings().removeClass('sel');
		n.eq(i).css('opacity',1).siblings().css('opacity',0.5);
		
		a.eq(i).show().siblings().hide();
	},function(){
		var maxs=n.length-1;
		var i=n.index(this);
		if(i<maxs){
			var ii=i+1;
		}
		else if(i==maxs){
			n.eq(0).addClass('sel').siblings().removeClass('sel');
			n.eq(0).css('opacity',1).siblings().css('opacity',0.5);
		}
		
		n.eq(ii).addClass('sel').siblings().removeClass('sel');
		n.eq(ii).css('opacity',1).siblings().css('opacity',0.5);
		a.eq(ii).show().siblings().hide();

		set=window.setInterval(	function au1(){
		ii<a.length-1?ii++:ii=0;
		a.eq(ii).show().siblings().hide();
		n.eq(ii).addClass('sel').siblings().removeClass('sel');
		n.eq(ii).css('opacity',1).siblings().css('opacity',0.5);
	},timeout);	
	});
	
	
	

	a.hover(function(){
		window.clearInterval(set);
		var i=a.index(this);
		n.eq(i).addClass('sel').siblings().removeClass('sel');
		n.eq(i).css('opacity',1).siblings().css('opacity',0.5);
		a.eq(i).show().siblings().hide();
		
	},function(){
		var maxs=a.length-1;
		var i=a.index(this);
		if(i<maxs){
			var ii=i+1;
		}
		else if(i==maxs){
			n.eq(0).addClass('sel').siblings().removeClass('sel');
			n.eq(0).css('opacity',1).siblings().css('opacity',0.5);
		}
		
		n.eq(ii).addClass('sel').siblings().removeClass('sel');
		n.eq(ii).css('opacity',1).siblings().css('opacity',0.5);
		a.eq(ii).show().siblings().hide();
		
		set=window.setInterval(	function au1(){
		ii<a.length-1?ii++:ii=0;
		a.eq(ii).show().siblings().hide();
		n.eq(ii).addClass('sel').siblings().removeClass('sel');
		n.eq(ii).css('opacity',1).siblings().css('opacity',0.5);
	},timeout);	
	});
	
	
	
	
	
	function au(){
		num<a.length-1?num++:num=0;
		a.eq(num).show().siblings().hide();
		n.eq(num).addClass('sel').siblings().removeClass('sel');
		n.eq(num).css('opacity',1).siblings().css('opacity',0.5);
	}
	
	
	
	
}

function notes(){
	var a=$('#notes dt a');
	var p=$('#notes dd p');
	p.hide();
	a.eq(0).addClass('sel');
	p.eq(0).show();
	
	a.hover(function(){
		var i=a.index(this);
		a.removeClass('sel');
		
		if($(this).attr('class')=='adtwo'){
			$(this).addClass('sel');
		}
		else if($(this).attr('class')=='adone'){
			$(this).next().removeClass('sel');
			$(this).addClass('sel');
		}
		
		p.eq(i).show().siblings().hide();
		
	},function(){
		
		if($(this).attr('class')=='adtwo'){
			$(this).removeClass('sel');
		}
		if($(this).attr('class')=='adone'){
			$(this).removeClass('sel');
		}
	});
}
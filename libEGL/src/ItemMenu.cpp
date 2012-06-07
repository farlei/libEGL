#include "engcomp_glib.h"

ItemMenu::ItemMenu(void)
{
	estado = 0;
	pressionado = 0;
	background = false;
	fnt = NULL;
}

ItemMenu::~ItemMenu(void)
{
	if(fnt) delete fnt;
}

ItemMenu::ItemMenu(int ID, string txt, int px, int py, int larg, int alt)
{
	id = ID;
	estado = 0;
	x = px;
	y = py;
	txtLarg = largura = larg;
	txtAlt = altura = alt;
	pressionado = 0;
	texto = txt;
	background = false;
	fnt = NULL;
	memset(corHover,200,3);
	memset(corNormal,255,3);
	memset(corFundoHover,32,3);
	memset(corFundoNormal,0,3);
}

void ItemMenu::setarFonte(string arqFonte, int fntSize, bool fundo)
{
	if(fnt) delete fnt;
	fnt = new fonte();
	fnt->carregar(arqFonte,fntSize);
	background = fundo;

	fnt->medir_texto(texto,&txtLarg,&txtAlt);

	tx = ((largura - txtLarg)/2)+x;
	ty = ((altura - txtAlt)/2)+y;
}

void ItemMenu::setarCorNormal(int tr, int tg, int tb, int br, int bg, int bb)
{
	corNormal[0] = tr;
	corNormal[1] = tg;
	corNormal[2] = tb;

	corFundoNormal[0] = br;
	corFundoNormal[1] = bg;
	corFundoNormal[2] = bb;
}

void ItemMenu::setarCorHover(int tr, int tg, int tb, int br, int bg, int bb)
{
	corHover[0] = tr;
	corHover[1] = tg;
	corHover[2] = tb;

	corFundoHover[0] = br;
	corFundoHover[1] = bg;
	corFundoHover[2] = bb;
}

void ItemMenu::desenha()
{
	switch(estado)
	{
	case 0:
		if(background) egl_retangulo_arredondado(x,y,x+largura,y+altura,corFundoNormal[0],corFundoNormal[1],corFundoNormal[2]);
		fnt->desenha_texto(texto,tx,ty,corNormal[0],corNormal[1],corNormal[2]);
		break;
	case 1:
		if(background) egl_retangulo_arredondado(x,y,x+largura,y+altura,corFundoNormal[0],corFundoNormal[1],corFundoNormal[2]);
		fnt->desenha_texto(texto,tx+1,ty+1,corNormal[0],corNormal[1],corNormal[2]);
		break;
	case 2:
		if(background) egl_retangulo_arredondado(x,y,x+largura,y+altura,corFundoHover[0],corFundoHover[1],corFundoHover[2]);
		fnt->desenha_texto(texto,tx,ty,corHover[0],corHover[1],corHover[2]);
		break;
	}
	
}

enum EGL_EVENTOS ItemMenu::processaEventos()
{
	if(dentroDoRetangulo(mouse_x, mouse_y) && (!pressionado))
	{
		if(mouse_b )
		{
			estado = 1;
			pressionado = 10;
			if( mouse_b & SDL_BUTTON(2)) return EGL_CLIQUE_MEIO;
			if( mouse_b & SDL_BUTTON(3)) return EGL_CLIQUE_DIR;
			return EGL_CLIQUE_ESQ;
		}
		estado = 2;
		return EGL_HOVER;
	}
	else
	{
		if(!pressionado) estado = 0;
	}
	if((!mouse_b) && pressionado) pressionado--;


	return EGL_VAZIO;
}

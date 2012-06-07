#include "engcomp_glib.h"

Botao::Botao(void)
{
	estado = 0;
	pressionado = 0;
}

Botao::Botao(int ID, int px, int py, string imgNormal, string imgClique, string imgHover)
{
	id = ID;
	estado = 0;
	x = px;
	y = py;
	imgs[0].carregar(imgNormal);
	largura = imgs[0].getResX();
	altura = imgs[0].getResY();
	imgClique.length() != 0 ? imgs[1].carregar(imgClique) : imgs[0].carregar(imgNormal);
	imgHover.length() != 0 ? imgs[2].carregar(imgHover) : imgs[0].carregar(imgNormal);
	pressionado = 0;
}

Botao::~Botao(void)
{
}

void Botao::desenha()
{
	imgs[estado].desenha(x,y);
}

enum EGL_EVENTOS Botao::processaEventos()
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

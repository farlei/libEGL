#include "engcomp_glib.h"


ComponenteBase::ComponenteBase(void)
{
	id = -1;
	x = 0;
	y = 0;
	largura = 50;
	altura = 50;
}

ComponenteBase::~ComponenteBase(void)
{
}

int ComponenteBase::getID(void)
{
	return id;
}

bool ComponenteBase::dentroDoRetangulo(int mx, int my)
{
	if( (mx >= x) && (mx <= (x+largura)) && (my >= y) && (my <= (y+altura)) )
		return true;
	return false;
}

void ComponenteBase::desenha()
{
	egl_retangulo_arredondado(x,y,x+largura,y+altura,255,255,255);
}

enum EGL_EVENTOS ComponenteBase::processaEventos()
{
	return EGL_VAZIO;
}

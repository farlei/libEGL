#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

int main(int argc, char* argv[])
{
	egl_inicializar(800,600,true);

	imagem tanque;
	tanque.carregar("tanque.png");

	TileMap mapa;
	mapa.inicializa("mapa.txt");


	float rot = 0;
	float x = 300;
	float y = 300;
	float vel = 1.2;
	float rad;
	const float pi180 = (3.1416/180);

	while(!key[SDLK_ESCAPE])
	{
		mapa.desenha();
		tanque.desenha_rotacionado(x,y,rot);

		//egl_depurar("X",x);
		//egl_depurar("Y",y);
		//egl_depurar("Rotacao",rot);

		if(key[SDLK_LEFT])
		{
			rot+=vel;
		}
		if(key[SDLK_RIGHT])
		{
			rot-=vel;
		}

		if(key[SDLK_UP])
		{
			rad = rot*pi180;
			x += cos(rad) * vel;
			y += -sin(rad) * vel;
		}
		if(key[SDLK_DOWN])
		{
			rad = rot*pi180;
			x -= cos(rad) * vel;
			y -= -sin(rad) * vel;
		}
		
		egl_desenha_frame();
	}

	egl_finalizar();

	return 0;
}
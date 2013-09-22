#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

#include "RoboLinha.h"
#include "Simulador.h"

int main(int argc, char* argv[])
{
	egl_inicializar(800,600,true);

	imagem mapa;
	mapa.carregar("pista01.png");

	RoboLinha rbtline(&mapa);
	Simulador simulador;
	simulador.adicionaRobo(&rbtline);

	while(!key[SDLK_ESCAPE])
	{
		mapa.desenha(0,0);

		simulador.executaPasso();

		if(key[SDLK_RETURN]) simulador.resume();
		if(key[SDLK_SPACE]) simulador.pausa();
		if(key[SDLK_r]) simulador.resetaTempo();

		egl_desenha_frame();
	}

	egl_finalizar();

	return 0;
}
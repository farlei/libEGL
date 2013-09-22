#include <iostream>
#include <string>
#include <sstream>
#include <time.h>
#include <iomanip>
#include "engcomp_glib.h"

using namespace std;

#include "RoboBase.h"
#include "Simulador.h"

Simulador::Simulador(void)
{
	pausado = true;
	texto.carregar("digital.ttf",48);
	resetaTempo();
}

Simulador::~Simulador(void)
{

}

void Simulador::contaTempo()
{
	ticks = clock();
}

void Simulador::mostraTempo()
{
	long delta = ticks - startTicks;
	long seg = delta / CLOCKS_PER_SEC;

	long min = seg/60;
	seg = seg - min*60;

	string txt;
	stringstream conv;
	conv  << min << ":" << seg << "   ( " << delta << " )";
	getline(conv,txt);

	texto.desenha_texto(txt,500,5,0,0,255);
}

void Simulador::resetaTempo()
{
	startTicks = clock();
	ticks = clock();
}

void Simulador::adicionaRobo(RoboBase* robo)
{
	robos.push_back(robo);
}

void Simulador::pausa()
{
	pausado = true;
	RoboBase* rbt = NULL;
	for(int i = 0; i < robos.size(); i++)
	{
		rbt = robos[i];
		rbt->setarMotorDir(0.0);
		rbt->setarMotorEsq(0.0);
	}
}

void Simulador::resume()
{
	pausado = false;
	resetaTempo();
}

void Simulador::executaPasso()
{
	RoboBase* rbt = NULL;
	for(int i = 0; i < robos.size(); i++)
	{
		rbt = robos[i];
		rbt->atualiza();
		if(!pausado) rbt->comportamento();
		rbt->desenha();
	}

	if ( (mouse_b & 1) && rbt)
	{
		rbt->resetaObjetivo();
		rbt->setPos(mouse_x,mouse_y);

		if(key[SDLK_UP]||key[SDLK_LEFT]) rbt->giraEsq();
		if(key[SDLK_DOWN]||key[SDLK_RIGHT]) rbt->giraDir();
		if(key[SDLK_BACKSPACE]) rbt->zeraDir();
	}

	if(!rbt->objetivoAtingido()) contaTempo();
	
	if(pausado)
	{
		texto.desenha_texto("PAUSADO",10,5,0,255,0);
	}
	else
	{
		mostraTempo();
	}
}

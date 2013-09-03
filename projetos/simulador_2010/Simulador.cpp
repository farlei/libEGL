#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

#include "RoboBase.h"
#include "Simulador.h"

Simulador::Simulador(void)
{

}

Simulador::~Simulador(void)
{

}

void Simulador::adicionaRobo(RoboBase* robo)
{
	robos.push_back(robo);
}

void Simulador::executaPasso()
{
	RoboBase* rbt = NULL;
	for(int i = 0; i < robos.size(); i++)
	{
		rbt = robos[i];
		rbt->atualiza();
		rbt->desenha();
	}

	if ( (mouse_b & 1) && rbt)
	{
		rbt->x = mouse_x;
		rbt->y = mouse_y;
	}
}

#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

#include "RoboLinha.h"

// Construtor (ao final chamando o construtor da classe Pai)
RoboLinha::RoboLinha(imagem* mapa) : RoboBase(100,100,0,mapa)
{	
}

string RoboLinha::arquivoImagemRobo()
{
	return "robo.png";
}

void RoboLinha::comportamento()
{
	const float vp = 0.5;
	setarMotorDir(vp);
	setarMotorEsq(vp);

	if(sensor[CENTRAL])
	{
		setarMotorDir(vp);
		setarMotorEsq(vp);
	}
	if(sensor[DIR])
	{
		setarMotorDir(0.0);
		setarMotorEsq(vp);
	}
	if(sensor[ESQ])
	{
		setarMotorDir(vp);
		setarMotorEsq(0.0);
	}

	if(sensorCor == VERDE)
	{
		setarMotorDir(0.0);
		setarMotorEsq(0.0);
	}
	
}



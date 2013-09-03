#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

#include "RoboBase.h"

RoboBase::RoboBase()
{
	x = 0.0;
	y = 0.0;     
	dir = 0.0*toRAD;
	
	m1 = 0.0;
	m2 = 0.0;

	r = 0.5; // raio
	L = 20.0; // largura entre rodas

	nSens = NSENS;

	robo = new imagem();
	robo->carregar(arquivoImagemRobo());

	ambiente = robo;

	snsOff = new imagem();
	snsOn = new imagem();
	snsOff->carregar("off.png");
	snsOn->carregar("on.png");
}

RoboBase::RoboBase(float px, float py, float dr, imagem* pamb)
{
	x = px;
	y = py;     
	dir = dr*toRAD;
	ambiente = pamb;

	m1 = 0.0;
	m2 = 0.0;

	r = 0.5; // raio
	L = 20.0; // largura entre rodas

	nSens = NSENS;

	robo = new imagem();
	robo->carregar(arquivoImagemRobo());

	snsOff = new imagem();
	snsOn = new imagem();
	snsOff->carregar("off.png");
	snsOn->carregar("on.png");
}

RoboBase::~RoboBase()
{
	delete robo;
	delete snsOff;
	delete snsOn;
}

void RoboBase::atualiza()
{
	Uint8 cr,cg,cb,ca;
	Uint32 color_value;

	// robo
	x = x + ((r/2.0)*(m1+m2)*cos(dir));
	y = y + ((r/2.0)*(m1+m2)*sin(dir));
	dir = dir + ((r/L)*(m1-m2));  

	// sensores
	sx[0] = x+27;
	sy[0] = y+0;

	sx[1] = x+25;
	sy[1] = y-8;

	sx[2] = x+25;
	sy[2] = y+8;

	sx[3] = x+20;
	sy[3] = y-16;

	sx[4] = x+20;
	sy[4] = y+16;

	for(int i = 0; i < nSens;i++)
	{       
		senx[i] = x + ( cos(dir) * (sx[i] - x) - sin(dir) * (sy[i] - y) );
		seny[i] = y + ( sin(dir) * (sx[i] - x) + cos(dir) * (sy[i] - y) );

		SDL_PixelFormat fmt;
		color_value = ambiente->obter_pixel(senx[i],seny[i],fmt);
		SDL_GetRGBA(color_value,&fmt,&cr,&cg,&cb,&ca);
		float gray = 0.2126 * cr + 0.7152 * cg + 0.0722 * cb;
		if(gray > 128)
			sensor[i] = false;
		else
			sensor[i] = true; 
	}
	this->comportamento();
}

void RoboBase::desenha()
{
	robo->desenha_rotacionado(x-32,y-32,-(dir*toDEG));

	for(int i = 0; i < nSens;i++)
	{
		if(sensor[i])
			snsOn->desenha((int)senx[i]-3,(int)seny[i]-3);
		else
			snsOff->desenha((int)senx[i]-3,(int)seny[i]-3);
		egl_pixel((int)senx[i],(int)seny[i],255,0,0);
	}

	egl_pixel((int)x,(int)y,255,255,255);
}

void RoboBase::setarMotorEsq(float velocidade)
{
	m1 = velocidade;
}

void RoboBase::setarMotorDir(float velocidade)
{
	m2 = velocidade;
}

string RoboBase::arquivoImagemRobo()
{
	return "robo.png";
}

void RoboBase::comportamento()
{
	setarMotorDir(0.0);
	setarMotorEsq(0.0);
}


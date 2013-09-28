#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

#include "RoboBase.h"

#define STEP_RAD (2.0*PI)/720.0 // 0.5 grau

RoboBase::RoboBase()
{
	x = 0.0;
	y = 0.0;     
	dir = 0.0*toRAD;
	sensorCor = PRETO;
	objetivo = false;
	
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
	sensorCor = PRETO;
	objetivo = false;

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

void RoboBase::processaCor(Uint8 cr,Uint8 cg,Uint8 cb)
{
	Uint8 delta = 32;
	sensorCor = PRETO;
	if( (cr-delta > cg) && (cr-delta > cb) ) sensorCor = VERMELHO;
	if( (cg-delta > cr) && (cg-delta > cb) ) sensorCor = VERDE;
	if( (cb-delta > cg) && (cb-delta > cr) ) sensorCor = AZUL;
	if(sensorCor == PRETO)
	{
		float gray = 0.2126 * cr + 0.7152 * cg + 0.0722 * cb;
		if(gray > 128) sensorCor = BRANCO;
	}
}

void RoboBase::mostraSensorCor(int cx, int cy)
{
	Uint8 cr,cg,cb;
	cr = ((sensorCor == VERMELHO)||(sensorCor == BRANCO)) ? 255 : 0;
	cg = ((sensorCor == VERDE)||(sensorCor == BRANCO)) ? 255 : 0;
	cb = ((sensorCor == AZUL)||(sensorCor == BRANCO)) ? 255 : 0;

	egl_pixel(cx,cy,cr,cg,cb);
	egl_pixel(cx+1,cy,cr,cg,cb);
	egl_pixel(cx,cy+1,cr,cg,cb);
	egl_pixel(cx+1,cy+1,cr,cg,cb);
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
	sx[CENTRAL] = x+27;
	sy[CENTRAL] = y+0;

	sx[ESQ] = x+25;
	sy[ESQ] = y-8;

	sx[DIR] = x+25;
	sy[DIR] = y+8;

	sx[EXT_ESQ] = x+20;
	sy[EXT_ESQ] = y-16;

	sx[EXT_DIR] = x+20;
	sy[EXT_DIR] = y+16;

	for(int i = 0; i < nSens;i++)
	{       
		senx[i] = x + ( cos(dir) * (sx[i] - x) - sin(dir) * (sy[i] - y) );
		seny[i] = y + ( sin(dir) * (sx[i] - x) + cos(dir) * (sy[i] - y) );

		SDL_PixelFormat fmt;
		color_value = ambiente->obter_pixel(senx[i],seny[i],fmt);
		SDL_GetRGBA(color_value,&fmt,&cr,&cg,&cb,&ca);
		if(i == CENTRAL) processaCor(cr,cg,cb);
		float gray = 0.2126 * cr + 0.7152 * cg + 0.0722 * cb;
		if(gray > 128)
			sensor[i] = false;
		else
			sensor[i] = true; 
	}
	if(sensorCor == VERDE) objetivo = true;
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
	mostraSensorCor((int)x,(int)y);

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

void RoboBase::resetaObjetivo()
{
	objetivo = false;
}
bool RoboBase::objetivoAtingido()
{
	return objetivo;
}

void RoboBase::giraEsq()
{
	dir += STEP_RAD;
}

void RoboBase::giraDir()
{
	dir -= STEP_RAD;
}

void RoboBase::zeraDir()
{
	dir = 0.0;
}

void RoboBase::setPos(float px, float py)
{
	x = px;
	y = py;
}


#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

#define PI 3.141592
#define toRAD PI / 180.0
#define toDEG 180.0 / PI

#define NSENS 3

using namespace std;

class robo
{
public:
	float x,y;
	float dir; // angulo em radianos

	float sx[NSENS];
	float sy[NSENS];

	float senx[NSENS];
	float seny[NSENS];
	bool sensor[NSENS];

	int nSens;

protected:          
	float m1, m2, L, r;

	imagem* ambiente;

public:
	robo(float px, float py, float dr, imagem* pamb)
	{
		x = px;
		y = py;     
		dir = dr*toRAD;
		ambiente = pamb;

		m1 = 0.0;
		m2 = 0.0;

		r = 0.5;
		L = 20.0;

		nSens = NSENS;


	}
	~robo()
	{
	}

	void executa()
	{
		Uint8 cr,cg,cb,ca;
		Uint32 color_value;

		// robo
		x = x + ((r/2.0)*(m1+m2)*cos(dir));
		y = y + ((r/2.0)*(m1+m2)*sin(dir));
		dir = dir + ((r/L)*(m1-m2));  

		// sensores
		sx[0] = x+25;
		sy[0] = y+0;

		sx[1] = x+25;
		sy[1] = y-8;

		sx[2] = x+25;
		sy[2] = y+8;

		for(int i = 0; i < nSens;i++)
		{       
			senx[i] = x + ( cos(dir) * (sx[i] - x) - sin(dir) * (sy[i] - y) );
			seny[i] = y + ( sin(dir) * (sx[i] - x) + cos(dir) * (sy[i] - y) );

			SDL_PixelFormat fmt;
			color_value = ambiente->obter_pixel(senx[i],seny[i],fmt);
			SDL_GetRGBA(color_value,&fmt,&cr,&cg,&cb,&ca);
			float gray = 0.2126 * cr + 0.7152 * cg + 0.0722 * cb;
			if(gray > 128)
				sensor[i] = true;
			else
				sensor[i] = false; 

		}   


	}

	void set_m1(float vel)
	{
		m1 = vel;
	}

	void set_m2(float vel)
	{
		m2 = vel;
	}

	void comportamento()
	{
		const float vp = 0.5;
		set_m2(vp);
		set_m1(vp);

		if(sensor[0])
		{
			set_m2(vp);
			set_m1(vp);
		}
		if(sensor[2])
		{
			set_m2(vp);
			set_m1(0.0);
		}
		if(sensor[1])
		{
			set_m2(0.0);
			set_m1(vp);
		}
		
	}
};


int main(int argc, char* argv[])
{
	float v1,v2;
	v1 = v2 = 0.0;

	egl_inicializar(800,600,true);

	imagem mapa;
	mapa.carregar("linha.png");

	imagem mouse;
	mouse.carregar("mouse.png");

	imagem tanque;
	tanque.carregar("robo.png");


	robo rbtline(100,60,0,&mapa);

	bool dbob = false;

	int rot;


	while(!key[SDLK_ESCAPE])
	{

		mapa.desenha(0,0);


		if ( (mouse_b & 1))
		{
			rbtline.x = mouse_x;
			rbtline.y = mouse_y;
		}

		rbtline.executa();
		rbtline.comportamento();

		tanque.desenha_rotacionado(rbtline.x-32,rbtline.y-32,-(rbtline.dir*toDEG));

		for(int i = 0; i < rbtline.nSens;i++)
		{
			egl_pixel((int)rbtline.senx[i],(int)rbtline.seny[i],255,0,0);
		}

		egl_pixel((int)rbtline.x,(int)rbtline.y,255,255,255);

		mouse.desenha(mouse_x,mouse_y);

		egl_desenha_frame();
	}

	egl_finalizar();

	return 0;
}
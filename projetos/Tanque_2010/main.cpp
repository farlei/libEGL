#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

int main(int argc, char* argv[])
{
	egl_inicializar(800,600,true);
	egl_limite_framerate(120); // limita em 30 frames por segundo (o padrão é de 60 fps)

	imagem tanque;
	tanque.carregar("tanque.png");

	TileMap mapa;
	//mapa.inicializa("mapa.txt");
	mapa.inicializaJSON("ruas.json","fundo");

	float rot = 0;
	float x = 300;
	float y = 300;
	float vel = 0.6;
	float rad;
	const float pi180 = (3.1416/180);

	Interface gui;
	gui.adicionaComponente(new Botao(0,650,450,"normal.png","clique.png","hover.png"));
	gui.adicionaComponente(new Botao(1,650,500,"normal.png","clique.png","hover.png"));

	ItemMenu* menu1 = new ItemMenu(2,"Texto do Item 1",20,450,200,35);
	menu1->setarFonte("fonte.ttf",16,true);
	ItemMenu* menu2 = new ItemMenu(3,"Texto do Item 2",20,490,200,35);
	menu2->setarFonte("fonte.ttf",16);
	gui.adicionaComponente(menu1);
	gui.adicionaComponente(menu2);

	int troca = 1;


	imagem explosao;
	explosao.carregar("explosao.png",0,0,32,32);
	explosao.carregar("explosao.png",32,0,32,32);
	explosao.carregar("explosao.png",32*2,0,32,32);
	explosao.carregar("explosao.png",32*3,0,32,32);
	explosao.carregar("explosao.png",32*4,0,32,32);
	explosao.carregar("explosao.png",32*5,0,32,32);
	explosao.carregar("explosao.png",32*6,0,32,32);

	int delay = 0;
	while(!key[SDLK_ESCAPE])
	{
		mapa.desenha();
		tanque.desenha_rotacionado(x,y,rot);

		gui.desenha();
		int btID = gui.verificaEventos(EGL_CLIQUE_DIR);
		if(btID == 0) troca *= (-1);

		int sair = gui.verificaEventosJanela(EGL_SAIR);
		if(sair == 1) break;

		// comandos que habilitam o painel de Debug
		// e atualizam os valores
		egl_depurar("X",x);
		egl_depurar("Y",y);
		egl_depurar("Rotacao Tanque",rot);
		egl_depurar("Delay",delay);
		egl_depurar("Clique",troca);

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
		
		// linha com espessura (último parametro indica a espessura)
		egl_linha(10,550,790,550,0,0,0,5);

		// desenha um retangulo com as bordas arredondadas
		egl_retangulo_arredondado(10,560,70,590,0,0,0,200);

		explosao.desenha(400,300);

		delay = egl_desenha_frame();
	}

	egl_finalizar();

	return 0;
}
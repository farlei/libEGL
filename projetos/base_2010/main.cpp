#include <iostream>
#include <string>
#include <sstream>
#include "engcomp_glib.h"

using namespace std;

int main(int argc, char* argv[])
{
	egl_inicializar(800,600,true);

	while(!key[SDLK_ESCAPE])
	{
		


		egl_desenha_frame();
	}

	egl_finalizar();

	return 0;
}
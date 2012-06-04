#include "engcomp_glib.h"

fonte::fonte()
{
	ii = jj = 0;
	font = NULL;
}

fonte::~fonte()
{
}

bool fonte::carregar(string arquivo, int size)
{
	if(!egl_init) return false;
	
	font = TTF_OpenFont(arquivo.c_str(),size);
	if(!font) return false;
	return true;
	
}

bool fonte::carregar_mem(unsigned char mem[], int memsize, int size)
{
	if(!egl_init) return false;

	SDL_RWops* rwops = SDL_RWFromMem((void*)mem,memsize);
	//SDL_RWops* rwops = SDL_RWFromConstMem((const void*)mem,memsize);
	if(!rwops) return false;

	font = TTF_OpenFontIndexRW(rwops,1,size,0);
	if(!font) return false;

	return true;
}

//Função que escreve o texto
void fonte::desenha_texto(string txt, int x, int y, int vermelho, int verde, int azul)
{
	if(!egl_init) return;

	SDL_Rect offset;
	SDL_Color color={vermelho,verde,azul};
	SDL_Surface *text_surface;
	text_surface=TTF_RenderText_Blended(font,txt.c_str(),color); 
	offset.x = x;
	offset.y = y;
	SDL_BlitSurface(text_surface,NULL,tela,&offset);
	SDL_FreeSurface(text_surface);
}

void fonte::medir_texto(string txt, int* tw, int* th)
{
	if(!egl_init) return;

	SDL_Color color={255,255,255};
	SDL_Surface *text_surface;
	text_surface=TTF_RenderText_Blended(font,txt.c_str(),color); 

	*tw = text_surface->w;
	*th = text_surface->h;
	
	SDL_FreeSurface(text_surface);
}



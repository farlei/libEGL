#include "engcomp_glib.h"
#include <algorithm>
#include <cctype>

Uint32 rmask, gmask, bmask, amask;

bool egl_init=false;
SDL_Surface* tela=NULL;

Uint8 *key;

int mouse_x;
int mouse_y;
Uint8 mouse_b;

Uint32 limiteFramerate;
Uint32 tempoAntes;

int res_x; 
int res_y;
Uint32 clear_color;
bool egl_debug=false;
string msg_erro;

// setup das threads SDL
bool kill_threads;
SDL_Thread* t_eventos;

#include "fonte.h"

bool egl_inicializar(int w, int h, bool janela)
{
	 // SDL interprets each pixel as a 32-bit number, so our masks must depend
     //  on the endianness (byte order) of the machine 
	#if SDL_BYTEORDER == SDL_BIG_ENDIAN
		rmask = 0xff000000;
		gmask = 0x00ff0000;
		bmask = 0x0000ff00;
		amask = 0x000000ff;
	#else
		rmask = 0x000000ff;
		gmask = 0x0000ff00;
		bmask = 0x00ff0000;
		amask = 0xff000000;
	#endif

	SDL_Init( SDL_INIT_VIDEO | SDL_INIT_AUDIO );

	// Audio: SDL_Mixer
	Mix_Init(MIX_INIT_MP3 | MIX_INIT_OGG | MIX_INIT_FLAC);
	int audio_rate = 22050;
	Uint16 audio_format = AUDIO_S16SYS;
	int audio_channels = 2;
	int audio_buffers = 1024; //4096
	if(Mix_OpenAudio(audio_rate, audio_format, audio_channels, audio_buffers) != 0) 
	{
		// ERRO
	}
	///////

	// Video
	tempoAntes = SDL_GetTicks();
	limiteFramerate = (Uint32)((1.0/60.0)*1000.0); // 60 frames por segundo
	if(!janela)
	{
		tela = SDL_SetVideoMode( w, h, 0, SDL_SWSURFACE | SDL_DOUBLEBUF | SDL_FULLSCREEN);
	}
	else
	{
		tela = SDL_SetVideoMode( w, h, 0, SDL_SWSURFACE | SDL_DOUBLEBUF );
		SDL_WM_SetCaption( "libEGL", 0 );
	}
	
	res_x = w; res_y = h;

	clear_color = SDL_MapRGB(tela->format, 0, 0, 0);
	SDL_FillRect(tela, NULL, clear_color);

	TTF_Init();

	egl_init = true;


	kill_threads = false;
	t_eventos = SDL_CreateThread(egl_processa_eventos,NULL);

	return true;
}

void egl_finalizar()
{
	kill_threads = true;
	SDL_WaitThread(t_eventos,NULL);

	if(tela) SDL_FreeSurface(tela);
	TTF_Quit();
	Mix_CloseAudio();
	while(Mix_Init(0)) Mix_Quit();
	SDL_Quit();
}

int  egl_processa_eventos(void* param)
{
	while(!kill_threads)
	{
		key = SDL_GetKeyState(NULL);
		mouse_b = SDL_GetMouseState(&mouse_x,&mouse_y);
	}
	return 1;
}

// egl4
void egl_limite_framerate(unsigned int maxFramerate) // frames por segundo
{
	limiteFramerate = (Uint32)((1.0/maxFramerate)*1000.0);
}

void egl_desenha_frame(bool limpa)
{
	if(!egl_init) return;
    if(egl_debug) egl_texto(msg_erro.c_str(),0,0);

	SDL_PumpEvents();

	int32_t delay = (tempoAntes + limiteFramerate) - SDL_GetTicks();
	if(delay <= 0) 
	{
		SDL_Flip(tela);
		if(limpa) 
		  SDL_FillRect(tela, NULL, clear_color);
		tempoAntes = SDL_GetTicks();
	}
}

void egl_pixel(int x,int y, int vermelho, int verde, int azul)
{
	if(!egl_init) return;

	pixelRGBA(tela,x,y,vermelho,verde,azul,255);
}

void egl_linha(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul)
{
	if(!egl_init) return;

	lineRGBA(tela,x1,y1,x2,y2,vermelho, verde, azul,255);
}

void egl_retangulo(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul)
{
	if(!egl_init) return;

	rectangleRGBA(tela,x1,y1,x2,y2,vermelho, verde, azul,255);
}

void egl_sleep(int milisec)
{
	SDL_Delay(milisec);
}

void egl_erro(string mensagem)
{
	msg_erro += (" " + mensagem);
}

void egl_texto(string txt, int x, int y, int cR, int cG, int cB)
{
	if(!egl_init) return;

	stringRGBA(tela,x,y,txt.c_str(),cR,cG,cB,255);
}

#include "engcomp_glib.h"
#include <algorithm>
#include <cctype>
#include <vector>
#include <string>
#include <sstream>

#define DEBUG_ALT 105

Uint32 rmask, gmask, bmask, amask;

bool egl_init=false;
SDL_Surface* tela=NULL;

Uint8 *key;

int mouse_x;
int mouse_y;
Uint8 mouse_b;

Uint32 limiteFramerate;
Uint32 tempoAntes;

SDL_Surface *painelDebug;

int res_x; 
int res_y;
Uint32 clear_color;

// depuração
bool egl_debug=false;
struct debIt
{
	string chave;
	string valor;
};
typedef struct debIt debugItem;
vector<debugItem*> debugMessages;


// setup das threads SDL
bool kill_threads;
SDL_Thread* t_eventos;

#include "fonte.h"

SDL_Surface *alphaRect(int width, int height, Uint8 red, Uint8 green, Uint8 blue)
{
	SDL_Surface *surface, *newImage;

	surface = SDL_CreateRGBSurface(SDL_SWSURFACE, width, height, 32, rmask, gmask, bmask, amask);

	if (surface == NULL) return NULL;

	newImage = SDL_DisplayFormat(surface);

	SDL_FillRect(newImage, NULL, SDL_MapRGB(newImage->format, red,
		green, blue));

	SDL_SetAlpha(newImage, SDL_SRCALPHA|SDL_RLEACCEL, 164);

	SDL_FreeSurface(surface);

	return newImage;
}

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

	painelDebug = alphaRect(res_x,DEBUG_ALT,0,0,0);

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

	debugItem* temp = NULL;
	for(int i = 0; i < debugMessages.size(); i++)
	{
		temp = debugMessages[i];
		delete temp;
	}
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

// egl4
void processaDebug()
{
	if(!egl_debug) return;
	if(debugMessages.size() == 0) return;

	SDL_Rect rect;
	rect.x = 0;
	rect.y = 0;
	rect.w = res_x;
	rect.h = DEBUG_ALT;
	SDL_BlitSurface(painelDebug,NULL,tela,&rect);

	egl_texto("libEGL 4 - Painel de DEBUG",2,2,255,255,255);

	int debY = 14;
	debugItem* temp = NULL;
	for(int i = 0; i < debugMessages.size(); i++)
	{
		if(i == 8) 
		{
			egl_texto("...",8,debY,164,164,164);
			break;
		}
		temp = debugMessages[i];
		egl_texto(temp->chave,8,debY,164,164,164);
		egl_texto(temp->valor,150,debY,164,164,164);
		debY += 10;
	}

}

// egl4
void egl_depurar(string chave, string valor)
{
	egl_debug = true;
	debugItem* deb = NULL;
	debugItem* temp = NULL;
	for(int i = 0; i < debugMessages.size(); i++)
	{
		temp = debugMessages[i];
		if(temp->chave == chave) deb = temp;
	}
	if(deb == NULL)
	{
		deb = new debugItem;
		deb->chave = chave;
		deb->valor = valor;
		debugMessages.push_back(deb);
	}
	else
	{
		deb->valor = valor;
	}
}

// egl4
void egl_depurar(string chave, int valor)
{
	string temp;
	stringstream conv;
	conv << valor;
	conv >> temp;
	egl_depurar(chave,temp);
}

// egl4
void egl_depurar(string chave, double valor)
{
	string temp;
	stringstream conv;
	conv << valor;
	conv >> temp;
	egl_depurar(chave,temp);
}

// egl4
void egl_depurar(string chave, float valor)
{
	string temp;
	stringstream conv;
	conv << valor;
	conv >> temp;
	egl_depurar(chave,temp);
}

// egl4
void egl_depurar(string chave, char valor)
{
	string temp;
	stringstream conv;
	conv << valor;
	conv >> temp;
	egl_depurar(chave,temp);
}

// egl4
void egl_erro(string mensagem)
{
	egl_debug = true;
	debugItem* deb;
	deb = new debugItem;
	deb->chave = "ERRO";
	deb->valor = mensagem;
	debugMessages.push_back(deb);
}

int32_t egl_desenha_frame(bool limpa)
{
	if(!egl_init) return 0;

	SDL_PumpEvents();

	processaDebug();

	int32_t delay = (tempoAntes + limiteFramerate) - SDL_GetTicks();
	if(delay <= 0) 
	{
		SDL_Flip(tela);
		tempoAntes = SDL_GetTicks();
	}
	if(limpa) SDL_FillRect(tela, NULL, clear_color);
	return delay;
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

// egl4
void egl_linha(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul, int largura)
{
	if(!egl_init) return;

	thickLineRGBA(tela,x1,y1,x2,y2,largura,vermelho, verde, azul,255);
}

void egl_retangulo(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul)
{
	if(!egl_init) return;

	rectangleRGBA(tela,x1,y1,x2,y2,vermelho, verde, azul,255);
}

// egl4
void egl_retangulo_arredondado(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul, int alpha)
{
	if(!egl_init) return;

	roundedBoxRGBA(tela,x1,y1,x2,y2,8,vermelho, verde, azul, alpha);
}

void egl_sleep(int milisec)
{
	SDL_Delay(milisec);
}

void egl_texto(string txt, int x, int y, int cR, int cG, int cB)
{
	if(!egl_init) return;

	stringRGBA(tela,x,y,txt.c_str(),cR,cG,cB,255);
}

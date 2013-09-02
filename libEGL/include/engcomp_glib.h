#pragma once

/*
Biblioteca de Desenvolvimento de Jogos do Curso de Engenharia da Computação da UNISINOS
by Farlei J. Heinen (30/05/2006) - farleih@gmail.com
versão 4 - 09/05/2012
*/

#include "library_version.h"
#include <string>
#include <vector>
#include <list>
#include "json/json.h"

using namespace std;



#define EGL_CALL_CONV _cdecl

extern Uint32 rmask, gmask, bmask, amask;

extern bool egl_init;
extern SDL_Surface* tela;

extern Uint8 *key;

extern int mouse_x;
extern int mouse_y;
extern Uint8 mouse_b;

extern Uint32 limiteFramerate;

extern int res_x; 
extern int res_y;
extern Uint32 clear_color;
extern bool egl_debug;


bool EGL_CALL_CONV egl_inicializar(int w, int h, bool janela = false );

void EGL_CALL_CONV egl_finalizar();

void EGL_CALL_CONV egl_limite_framerate(unsigned int maxFramerate);

int32_t EGL_CALL_CONV egl_desenha_frame(bool limpa = true);

void EGL_CALL_CONV egl_pixel(int x,int y, int vermelho, int verde, int azul);

int EGL_CALL_CONV egl_processa_eventos(void* param);

void EGL_CALL_CONV egl_sleep(int milisec);

void EGL_CALL_CONV egl_erro(string mensagem);

void EGL_CALL_CONV egl_depurar(string chave, string valor);
void EGL_CALL_CONV egl_depurar(string chave, int valor);
void EGL_CALL_CONV egl_depurar(string chave, double valor);
void EGL_CALL_CONV egl_depurar(string chave, float valor);
void EGL_CALL_CONV egl_depurar(string chave, char valor);

void EGL_CALL_CONV egl_linha(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul);
void EGL_CALL_CONV egl_linha(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul, int largura);

void EGL_CALL_CONV egl_retangulo(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul);
void EGL_CALL_CONV egl_retangulo_arredondado(int x1,int y1, int x2,int y2, int vermelho, int verde, int azul, int alpha = 255);

void EGL_CALL_CONV egl_texto(string txt, int x, int y, int cR=255, int cG=255, int cB=255);

///////////////////////////////////////////////////////////////////////////////////////

// Classe fonte bitmap: contribuicao de Luan Carlos Nesi
// 18/11/2008
// Classe fonte alterada para TrueType: Farlei Heinen
// 07/07/2009
class fonte
{
private:
	int ii,jj;
	TTF_Font *font;

public:
	fonte();
	~fonte();

	bool carregar(string arquivo,int size=16);
	bool carregar_mem(unsigned char mem[],int memsize, int size=16);
	void desenha_texto(string txt, int x, int y, int vermelho=255, int verde=255, int azul=255);
	void medir_texto(string txt, int* tw, int* th);
};


class imagem
{
protected:
	vector<SDL_Surface*> bmp;
	int index;
	int curr;
	int tempo;
	int vel;

	int type; // 0:normal, 1:PNG
	bool decl_global; // indica declaracao global

	SDL_Rect pos;

public:
	imagem(const imagem& cp);
	imagem();
	~imagem();

	imagem& operator=(const imagem &r);
	bool operator==(const imagem &r);
	bool operator!=(const imagem &r);

	SDL_Surface* obter_bitmap();
	Uint32 obter_pixel(int x, int y, SDL_PixelFormat& pFormat);
	void obter_tamanho(int &w, int &h);
	void obter_dimensoes(int &altura, int &largura, unsigned int index);
	int getResX();
	int getResY();
	void setar_tempo_animacao(int veloc);
	void setGlobal(bool global=true);
	bool carregar(string arquivo, bool global=false);	
	bool carregar(string arquivo, int x, int y, int largura, int altura);
	bool desenha(int x, int y, bool borda=false);
	bool desenha_transparente(int x, int y, int trans=128);
	bool desenha_rotacionado(int x, int y, long rotacao );
	bool desenha_espelhado(int x, int y, bool horiz=true, bool vert=false );
	bool colide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2);
	bool colide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2, imagem &sprite2);
	bool colide(int x1, int y1, int x2, int y2, imagem &sprite2);
	
	void clonarBitmap(const imagem& cp);
protected:
	// // atributo de status para indicar falha 
	bool falha;
	// atributo com a mensagem relacionada com a falha
	string falha_str;
};

// SDL_Mixer
#define SAMPLE Mix_Chunk

class som
{
protected:
	SAMPLE *smp;

	int volume;
	int posicao;
	int frequencia;

	int voice;
    
public:
	som();
	som(const som &r);
	som& operator=(const som &r);
	bool operator==(const som &r);
	bool operator!=(const som &r);
	~som();
	bool carregar(std::string arquivo);
	void tocar(int repetir = 0);
	void parar();
	void ajustar(int vol, int pan=128, int freq=1000, int loop=0);
	bool final();
private:
	std::string nomeArquivo;
};

class musica
{
protected:
	Mix_Music *smp;

	int volume;
	int posicao;
	int frequencia;
    
public:
	musica();
	musica(const musica &r);
	musica& operator=(const musica &r);
	bool operator==(const musica &r);
	bool operator!=(const musica &r);
	~musica();
	bool carregar(std::string arquivo);
	void tocar(int repetir = 0);
	void parar();
	void ajustar(int vol, int pan=128, int freq=1000, int loop=0);
	bool final();
private:
	std::string nomeArquivo;
};

#include "TileEngine.h"

// interface 
#include "ComponenteBase.h"
#include "Botao.h"
#include "ItemMenu.h"
#include "Interface.h"
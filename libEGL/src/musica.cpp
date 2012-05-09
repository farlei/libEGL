#include "engcomp_glib.h"
#include <algorithm>

musica::musica() : smp(0), volume(255), posicao(128), frequencia(1000)
{
}

musica::musica(const musica &r)
{
	volume = r.volume;
	posicao = r.posicao;
	frequencia = r.frequencia;
	nomeArquivo =  r.nomeArquivo;
	carregar(r.nomeArquivo);
}
musica& musica::operator=(const musica &r)
{
	if(*this != r)
	{
		volume = r.volume;
		posicao = r.posicao;
		frequencia = r.frequencia;
		nomeArquivo =  r.nomeArquivo;
		carregar(r.nomeArquivo);
	}
	return *this;
}
bool musica::operator==(const musica &r)
{
	if(volume == r.volume && posicao == r.posicao && frequencia == r.frequencia && nomeArquivo ==  r.nomeArquivo)
		return true;
	else
		return false;
}
bool musica::operator!=(const musica &r)
{
	if(volume != r.volume || posicao != r.posicao || frequencia != r.frequencia || nomeArquivo !=  r.nomeArquivo)
		return true;
	else
		return false;
}
musica::~musica()
{
	if(smp) Mix_FreeMusic(smp);
}

bool musica::carregar(string arquivo)
{
	if(!egl_init) 
		return false;
	
	if(smp)
	{
		Mix_FreeMusic(smp);
	}
	smp = Mix_LoadMUS(arquivo.c_str());
	
	if(!smp) 
	{
		egl_erro("Erro carregando arquivo: " + arquivo + " - " + SDL_GetError());
		egl_debug = true;
		return false;
	}
	return true;
}
void musica::tocar(int repetir)
{
	if(!smp) return;

	//Mix_PlayMusic(smp,repetir == 0 ? 0 : -1);
	Mix_FadeInMusic(smp,repetir == 0 ? 0 : -1,50);

	
}
void musica::parar()
{
	if(!smp) return;

	Mix_HaltMusic();
}
void musica::ajustar(int vol, int pan, int freq, int loop)
{		
	if(!smp) return;
	//adjust_sample(smp, vol,  pan, freq, loop);
	volume = vol;
	posicao = pan;
	frequencia = freq;
}

// retornar se o som terminou de tocar
bool musica::final()
{
	if(!smp) return false;

	int status = Mix_PlayingMusic();
	if(status == 0) return true;
	
	return false;
}
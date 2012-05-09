#include "engcomp_glib.h"
#include <algorithm>

som::som()
:smp(0), volume(255), posicao(128), frequencia(1000), voice(-1)
{
}
som::som(const som &r)
{
	volume = r.volume;
	posicao = r.posicao;
	frequencia = r.frequencia;
	nomeArquivo =  r.nomeArquivo;
	carregar(r.nomeArquivo);
	voice = r.voice;
}
som& som::operator=(const som &r)
{
	if(*this != r)
	{
		volume = r.volume;
		posicao = r.posicao;
		frequencia = r.frequencia;
		nomeArquivo =  r.nomeArquivo;
		carregar(r.nomeArquivo);
		voice = r.voice;
	}
	return *this;
}
bool som::operator==(const som &r)
{
	if(volume == r.volume && posicao == r.posicao && frequencia == r.frequencia && nomeArquivo ==  r.nomeArquivo)
		return true;
	else
		return false;
}
bool som::operator!=(const som &r)
{
	if(volume != r.volume || posicao != r.posicao || frequencia != r.frequencia || nomeArquivo !=  r.nomeArquivo)
		return true;
	else
		return false;
}
som::~som()
{
	if(smp) Mix_FreeChunk(smp);
}

bool som::carregar(string arquivo)
{
	if(!egl_init) 
		return false;
	
	if(smp)
	{
		Mix_FreeChunk(smp);
		voice = -1;
	}
	smp = Mix_LoadWAV(arquivo.c_str());
	
	if(!smp) 
	{
		egl_erro("Erro carregando arquivo: " + arquivo + " - " + SDL_GetError());
		egl_debug = true;
		return false;
	}
	return true;
}
void som::tocar(int repetir)
{
	if(!smp) return;

	voice = Mix_PlayChannel(-1,smp,repetir == 0 ? 0 : -1);
}
void som::parar()
{
	if(!smp) return;
	
	if (voice != -1)
		Mix_HaltChannel(voice);
}
void som::ajustar(int vol, int pan, int freq, int loop)
{		
	if(!smp) return;
	//adjust_sample(smp, vol,  pan, freq, loop);
	volume = vol;
	posicao = pan;
	frequencia = freq;
}

// retornar se o som terminou de tocar
bool som::final()
{
	if(!smp) return false;
	if(voice == -1) return false;

	int status = Mix_Playing(voice);
	if(status == 0) return true;
	
	return false;
}
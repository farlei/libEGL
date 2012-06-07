#include "engcomp_glib.h"
#include <algorithm>

Interface::Interface(void)
{
}

Interface::~Interface(void)
{
	for(int i = 0; i < componentes.size(); i++)
	{
		delete componentes[i];
	}
}

void Interface::desenha(void)
{
	for(int i = 0; i < componentes.size(); i++)
	{
		componentes[i]->desenha();
	}
}

ComponenteBase* Interface::buscaComponente(int compID)
{
	for(int i = 0; i < componentes.size(); i++)
		if(componentes[i]->getID() == compID) return componentes[i];

	return NULL;
}

bool Interface::removeComponente(int compID)
{
	ComponenteBase* rem = buscaComponente(compID);
	if(rem)
	{
		vector<ComponenteBase*>::iterator pos = find(componentes.begin(),componentes.end(),rem);
		componentes.erase(pos);
		delete rem; 
		return true;
	}
	return false;
}

void Interface::adicionaComponente(ComponenteBase* componente)
{
	if(componente->getID() < 0) return;
	if(!buscaComponente(componente->getID()))
	{
		componentes.push_back(componente);
	}
}

int Interface::verificaEventosJanela(enum EGL_EVENTOS tipo)
{
	if(tipo == EGL_SAIR)
	{
		if(SDL_QuitRequested()) return 1;
	}
	return 0;
}

int Interface::verificaEventos(enum EGL_EVENTOS tipo)
{
	enum EGL_EVENTOS ret;
	for(int i = 0; i < componentes.size(); i++)
	{
		ret = componentes[i]->processaEventos();
		if(ret == tipo) return componentes[i]->getID();
	}
	return -1;
}
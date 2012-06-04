#include "engcomp_glib.h"


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

void Interface::adicionaComponente(ComponenteBase* componente)
{
	if(componente->getID() < 0) return;
	if(!buscaComponente(componente->getID()))
	{
		componentes.push_back(componente);
	}
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
#pragma once

enum EGL_EVENTOS {EGL_VAZIO, EGL_CLIQUE_ESQ, EGL_CLIQUE_DIR, EGL_CLIQUE_MEIO, EGL_HOVER, EGL_EDITADO, EGL_SAIR};

class Interface
{
private:
	vector<ComponenteBase*> componentes;
public:
	Interface(void);
	~Interface(void);

	void desenha(void);
	ComponenteBase* buscaComponente(int compID);
	bool removeComponente(int compID);
	void adicionaComponente(ComponenteBase* componente);
	int verificaEventos(enum EGL_EVENTOS tipo = EGL_CLIQUE_ESQ);
	int verificaEventosJanela(enum EGL_EVENTOS tipo);
};


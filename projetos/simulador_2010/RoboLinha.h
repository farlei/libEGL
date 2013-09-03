#pragma once

#include "RoboBase.h"

class RoboLinha : public RoboBase
{
public:
	RoboLinha(imagem* mapa);
	string arquivoImagemRobo();
	void comportamento();
};


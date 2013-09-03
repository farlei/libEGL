#pragma once

#define PI 3.141592
#define toRAD PI / 180.0
#define toDEG 180.0 / PI

#define NSENS 5

class RoboBase
{
public:

	float x,y;

protected:

	float dir; // angulo em radianos

	float sx[NSENS];
	float sy[NSENS];

	float senx[NSENS];
	float seny[NSENS];
	bool sensor[NSENS];

	int nSens;

	float m1, m2, L, r;

	imagem* ambiente;
	imagem* robo;
	imagem* snsOn;
	imagem* snsOff;

public:
	RoboBase();
	RoboBase(float px, float py, float dr, imagem* pamb);
	~RoboBase();

	void atualiza();
	void desenha();
	void setarMotorEsq(float velocidade);
	void setarMotorDir(float velocidade);

	virtual string arquivoImagemRobo();
	virtual void comportamento();
};
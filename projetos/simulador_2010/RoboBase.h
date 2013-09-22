#pragma once

#define PI 3.141592
#define toRAD PI / 180.0
#define toDEG 180.0 / PI

#define CENTRAL 0
#define ESQ 1
#define DIR 2
#define EXT_ESQ 3
#define EXT_DIR 4

typedef enum cores_ 
{
	PRETO,
	BRANCO,
	VERDE,
	AZUL,
	VERMELHO
} Cor ;

#define NSENS 5

class RoboBase
{
protected:
	float x,y;

	float dir; // angulo em radianos

	float sx[NSENS];
	float sy[NSENS];

	float senx[NSENS];
	float seny[NSENS];
	bool sensor[NSENS];
	Cor sensorCor;

	int nSens;

	float m1, m2, L, r;

	imagem* ambiente;
	imagem* robo;
	imagem* snsOn;
	imagem* snsOff;

private:

	bool objetivo;

public:
	RoboBase();
	RoboBase(float px, float py, float dr, imagem* pamb);
	~RoboBase();

	void processaCor(Uint8 cr,Uint8 cg,Uint8 cb);
	void mostraSensorCor(int cx, int cy);

	void setPos(float px, float py);
	void giraEsq();
	void giraDir();
	void zeraDir();
	void resetaObjetivo();
	bool objetivoAtingido();

	void atualiza();
	void desenha();
	void setarMotorEsq(float velocidade);
	void setarMotorDir(float velocidade);

	virtual string arquivoImagemRobo();
	virtual void comportamento();
};
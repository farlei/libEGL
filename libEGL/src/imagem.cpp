#include "engcomp_glib.h"

#include <algorithm>
#include <cctype>



imagem::imagem()
	:index(-1), curr(0), vel(30), tempo(30), falha(false), type(0), decl_global(false)
{
}

imagem::imagem(const imagem& cp) // construtor de cópia
	: falha(false)
{
	index = cp.index;
	curr = cp.curr;
	vel = cp.vel;
	tempo = cp.tempo;
	falha = cp.falha;
	falha_str = cp.falha_str;
	type = cp.type;
	decl_global = cp.decl_global;

	clonarBitmap(cp);
}

imagem& imagem::operator=(const imagem &r)
{
	if(*this != r)
	{
		index = r.index;
		curr = r.curr;
		vel = r.vel;
		tempo = r.tempo;
		falha = r.falha;
		falha_str = r.falha_str;
		type = r.type;
		decl_global = r.decl_global;

		clonarBitmap(r);
	}
	return *this;
}
bool imagem::operator==(const imagem &r)
{
	if(index == r.index && curr == r.curr && vel == r.vel && tempo == r.tempo && type == r.type)
		return true;
	else
		return false;
}
bool imagem::operator!=(const imagem &r)
{
	if(index != r.index || curr != r.curr || vel != r.vel || tempo != r.tempo || type != r.type)
		return true;
	else
		return false;
}

imagem::~imagem()
{
	if(!decl_global)
	{
		for(int i =0;i <= index;i++)
			if(bmp[i]) SDL_FreeSurface(bmp[i]);
	}
}

void imagem::setGlobal(bool global)
{
	decl_global = global;
}

SDL_Surface* imagem::obter_bitmap()
{
	if(!egl_init) return NULL;
	if(index < 0) return NULL;

	return bmp[curr];
}

Uint32 imagem::obter_pixel(int x, int y, SDL_PixelFormat& pFormat)
{
	SDL_Surface* surface = obter_bitmap();
	pFormat = *(surface->format);

	int w = getResX();
	int h = getResY();
	if( (x < 0) || (x >= w) ) return 0;
	if( (y < 0) || (y >= h) ) return 0;

	int bpp = surface->format->BytesPerPixel;
	Uint8 *p = (Uint8 *)surface->pixels + y * surface->pitch + x * bpp;

	switch(bpp) 
	{
	case 1:
		return *p;
		break;

	case 2:
		return *(Uint16 *)p;
		break;

	case 3:
		if(SDL_BYTEORDER == SDL_BIG_ENDIAN)
			return p[0] << 16 | p[1] << 8 | p[2];
		else
			return p[0] | p[1] << 8 | p[2] << 16;
		break;

	case 4:
		return *(Uint32 *)p;
	}
	return 0;
}

void imagem::obter_tamanho(int &w, int &h)
{
	if(!egl_init) return;
	if(index < 0) return;

	w = bmp[0]->w;
	h = bmp[0]->h;
}

void imagem::obter_dimensoes(int &altura, int &largura, unsigned int index)
{
	if(!egl_init || index < 0) 
		return;
	largura = bmp[index]->w;
	altura = bmp[index]->h;
}

int imagem::getResX()
{
	if(!egl_init) return -1;
	if(index < 0) return -1;

	return bmp[0]->w;
}

int imagem::getResY()
{
	if(!egl_init) return -1;
	if(index < 0) return -1;

	return bmp[0]->h;
}

void imagem::setar_tempo_animacao(int veloc)
{
	vel = tempo = veloc;
}

bool imagem::carregar(string arquivo, bool global)
{
	if(!egl_init)
	{
		falha = true;
		falha_str = "sem egl_inicializar() antes de tentar carregar:" + arquivo;
		return false;
	}

	decl_global = global;

	index++;
	SDL_Surface* btemp;
	btemp = IMG_Load(arquivo.c_str());


	if(!btemp) 
	{
		index--;
		string s_err = "Erro carreg. arq: " + arquivo;
		egl_erro(s_err);
		egl_debug = true;
		falha = true;
		falha_str = s_err;
		return false;
	}

	SDL_SetColorKey(btemp,SDL_SRCCOLORKEY,SDL_MapRGB(tela->format, 255, 0, 255) );
	SDL_Surface* remove = btemp;
	btemp = SDL_DisplayFormatAlpha(btemp);
	SDL_FreeSurface(remove);

	bmp.push_back(btemp);

	return true;
}

bool imagem::carregar(string arquivo, int x, int y, int largura, int altura)
{
	if(!egl_init)
	{
		falha = true;
		falha_str = "sem egl_inicializar() antes de tentar carregar:" + arquivo;
		return false;
	}

	index++;
	SDL_Surface* btemp;
	SDL_Surface* bmp_temp;

	bmp_temp = IMG_Load(arquivo.c_str());
	SDL_SetAlpha(bmp_temp,0,0);

	if(!bmp_temp) 
	{
		index--;
		string s_err = "Erro carreg. arq: " + arquivo;
		egl_erro(s_err);
		falha = true;
		falha_str = s_err;
		return false;
	}

	btemp = SDL_CreateRGBSurface(SDL_SWSURFACE, largura, altura, 32, rmask, gmask, bmask, amask);

	SDL_Rect r_orig;
	r_orig.x = x;
	r_orig.y = y;
	r_orig.w = largura;
	r_orig.h = altura;
	SDL_BlitSurface(bmp_temp,&r_orig,btemp,NULL);

	SDL_FreeSurface(bmp_temp);

	SDL_SetColorKey(btemp,SDL_SRCCOLORKEY,SDL_MapRGB(tela->format, 255, 0, 255) );
	SDL_Surface* remove = btemp;
	btemp = SDL_DisplayFormatAlpha(btemp);
	SDL_FreeSurface(remove);

	bmp.push_back(btemp);

	return true;
}

bool imagem::desenha(int x, int y, bool borda)
{
	if(!egl_init) return false;
	if( (index < 0) && (!falha) ) return false;

	if(falha)
	{
		egl_texto(falha_str,x,y,255,0,0);
		return false;
	}

	pos.x = x;
	pos.y = y;
	SDL_BlitSurface(bmp[curr],NULL,tela,&pos);

	if(borda) egl_retangulo(x,y,x+bmp[curr]->w,y+bmp[curr]->h,255,255,255);

	tempo--;
	if(!tempo)
	{
		curr++;
		tempo = vel;
		if(curr > index) 
		{
			curr = 0;
			return false;
		}
	}
	return true;
}

// Funciona somente se a imagem nao possuir canal alpha
bool imagem::desenha_transparente(int x, int y, int trans)
{
	if(!egl_init) return false;
	if( (index < 0) && (!falha) ) return false;

	if(falha)
	{
		egl_texto(falha_str,x,y,255,0,0);
		return false;
	}

	pos.x = x;
	pos.y = y;
	SDL_SetAlpha(bmp[curr],SDL_SRCALPHA,trans);
	SDL_BlitSurface(bmp[curr],NULL,tela,&pos);
	SDL_SetAlpha(bmp[curr],SDL_SRCALPHA,255);

	tempo--;
	if(!tempo)
	{
		curr++;
		tempo = vel;
		if(curr > index) 
		{
			curr = 0;
			return false;
		}
	}
	return true;
}

bool imagem::desenha_rotacionado(int x, int y, long rotacao )
{
	if(!egl_init) return false;
	if( (index < 0) && (!falha) ) return false;

	if(falha)
	{
		egl_texto(falha_str,x,y,255,0,0);
		return false;
	}

	SDL_Surface *imgrot = rotozoomSurface(bmp[curr],(double)rotacao,1,0);

	int deltah = (imgrot->h - bmp[curr]->h);
	int deltaw = (imgrot->w - bmp[curr]->w);

	x = x - deltaw/2;
	y = y - deltah/2;

	pos.x = x;
	pos.y = y;

	SDL_BlitSurface(imgrot,NULL,tela,&pos);
	SDL_FreeSurface(imgrot);

	tempo--;
	if(!tempo)
	{
		curr++;
		tempo = vel;
		if(curr > index) 
		{
			curr = 0;
			return false;
		}
	}
	return true;
}

bool imagem::desenha_espelhado(int x, int y, bool horiz, bool vert)
{
	/*
	if(!egl_init) return false;
	if( (index < 0) && (!falha) ) return false;

	if(falha)
	{
	egl_texto(falha_str,x,y,255,0,0);
	return false;
	}


	if(horiz && vert)
	{
	draw_sprite_vh_flip(tela,bmp[curr],x,y);
	}
	else
	{
	if(horiz) draw_sprite_h_flip(tela,bmp[curr],x,y);
	if(vert) draw_sprite_v_flip(tela,bmp[curr],x,y);
	}

	tempo--;
	if(!tempo)
	{
	curr++;
	tempo = vel;
	if(curr > index) 
	{
	curr = 0;
	return false;
	}
	}
	*/
	return true;
}

bool imagem::colide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
{
	return (!( ((x1)>=(x2)+(w2)) || 
		((x2)>=(x1)+(w1)) || 
		((y1)>=(y2)+(h2)) || 
		((y2)>=(y1)+(h1)) ));
}


bool imagem::colide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2, imagem &sprite2)
{
	if(!egl_init) return false;
	if(index < 0) return false;

	if(!colide(x1,y1,w1,h1,x2,y2,w2,h2)) return false;

	return (bool)SDL_CollidePixel(bmp[curr],x1,y1,  sprite2.obter_bitmap(),x2,y2);
}

bool imagem::colide(int x1, int y1, int x2, int y2, imagem &sprite2)
{
	if(!egl_init) return false;
	if(index < 0) return false;

	int w1 = bmp[curr]->w;
	int h1 = bmp[curr]->h;

	SDL_Surface* spr2 = sprite2.obter_bitmap();
	int w2 = spr2->w;
	int h2 = spr2->h;

	if(!colide(x1,y1,w1,h1,x2,y2,w2,h2)) return false;

	return (bool)SDL_CollidePixel(bmp[curr],x1,y1,  spr2,x2,y2);
}


void imagem::clonarBitmap(const imagem& cp)
{
	SDL_Surface* btemp;
	for(int i =0;i < (int)cp.bmp.size();i++)
	{
		btemp = SDL_CreateRGBSurface(SDL_SWSURFACE, cp.bmp[i]->w, cp.bmp[i]->h, cp.bmp[i]->format->BitsPerPixel, rmask, gmask, bmask, amask);
		SDL_BlitSurface(cp.bmp[i],NULL,btemp,NULL);
		bmp.push_back(btemp);
	}
}

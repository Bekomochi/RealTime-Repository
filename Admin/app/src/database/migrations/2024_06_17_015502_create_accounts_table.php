<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    /**
     * Run the migrations.
     */

    //テーブル作成時
    public function up(): void
    {
        //テーブルのカラム構成を指定
        Schema::create('accounts', function (Blueprint $table) {
            $table->id();                             //idカラム
            $table->string('name', 20)->unique();//nameカラム
            $table->string('password', 100);//nameカラム

            $table->timestamps();                    //created/update-at

            //$table->unique('name');//nameにユニーク制約設定
            //$table->index(○○)でindex設定
        });
    }

    /**
     * Reverse the migrations.
     */
    //テーブル削除時
    public function down(): void
    {
        Schema::dropIfExists('accounts');
    }
};

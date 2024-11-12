<?php

namespace App\Http\Controllers;

use App\Http\Resources\ItemResource;
use App\Models\Item;
use Illuminate\Http\Request;

class ItemController extends Controller
{
    //
    public function item(Request $request)
    {
        //ログアウト状態だったらログイン画面へ遷移
        if (!$request->session()->exists('login')) {
            return redirect('auth/login');
        }
        $items = Item::all();
        return view('Admin.itemView', ['items' => $items]);//アイテム一覧を表示
        /*return response()->json(
            ItemResource::make($items)
        );*/
    }
}

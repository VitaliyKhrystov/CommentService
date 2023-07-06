export class CommenttModelRequest{
  topicURL!: string;
  parrentId!: string;
  userId!: string;
  commentText!: string;
}

export class CommentModelResponse{
  commentId!: string;
  parrentId!: string;
  userId!: string;
  commentText!: string;
  createdAt!: Date;
  updatedAt!: Date;
  likes!: Like[];
  disLikes!: DisLike[];
  replies!: CommentModelResponse[];
}

export class Action{
  id!: string;
  commentId!: string;
  userId!: string;
  createdAt!: Date;
}
export class Like extends Action{}

export class DisLike extends Action{}
